import React, { useEffect, useState } from "react";
import { NavLink, useLocation } from "react-router-dom";
import {
  ChevronDown,
  ChevronRight,
  GraduationCap,
  Circle,
} from "lucide-react";
import { cn } from "@/lib/utils";
import { useAuth } from "@/contexts/AuthContext";
import { useSidebar } from "@/components/ui/sidebar";
import {
  Sidebar,
  SidebarContent,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar";
import { MenuItem, getMenus } from "@/services/MenuService";
import { buildMenuTree } from "@/utils/menuUtils";
import * as LucideIcons from "lucide-react";

// --- Helper to dynamically map icon name from API to Lucide Icon ---
function getIconByName(name?: string): React.ComponentType<{ className?: string }> {
  if (!name) return Circle;
  const formattedName = name
    .replace(/[-_ ](\w)/g, (_, c) => c.toUpperCase()) // convert kebab-case to PascalCase
    .replace(/^(\w)/, (_, c) => c.toUpperCase());
  const icon = (LucideIcons as any)[formattedName];
  return icon || Circle;
}

interface NavItemProps {
  item: MenuItem;
  level?: number;
}

/**
 * Recursive Nav Item component
 */
function NavItemComponent({ item, level = 0 }: NavItemProps) {
  const location = useLocation();
  const [isOpen, setIsOpen] = useState(false);
  const { state } = useSidebar();
  const isCollapsed = state === "collapsed";
  const Icon = getIconByName(item.icon);

  const isActive = location.pathname === item.route;
  const isChildActive =
    item.subMenus?.some((child) => location.pathname.startsWith(child.route ?? "")) || false;

  const hasChildren = item.subMenus && item.subMenus.length > 0;

  useEffect(() => {
    if (isChildActive) {
      setIsOpen(true);
    }
  }, [isChildActive]);

  // --- Sort children based on sortOrder ---
  const sortedSubMenus = hasChildren
    ? [...item.subMenus!].sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0))
    : [];

  // --- Parent menu with submenus ---
  if (hasChildren) {
    return (
      <SidebarMenuItem>
        <SidebarMenuButton
          onClick={() => setIsOpen((prev) => !prev)}
          className={cn(
            "w-full justify-between transition-colors rounded-md",
            (isActive || isChildActive) && "bg-brand-primary text-white"
          )}
        >
          <div className="flex items-center">
            <Icon
              className={cn(
                "mr-3 h-4 w-4 shrink-0 transition-colors",
                (isActive || isChildActive) && "text-white"
              )}
            />
            {!isCollapsed && <span>{item.displayName}</span>}
          </div>
          {!isCollapsed &&
            (isOpen ? (
              <ChevronDown className="h-4 w-4" />
            ) : (
              <ChevronRight className="h-4 w-4" />
            ))}
        </SidebarMenuButton>

        {/* --- Recursive Submenus --- */}
        {isOpen && !isCollapsed && (
          <div className="ml-4 mt-1 space-y-1">
            {sortedSubMenus.map((child) => (
              <NavItemComponent key={child.id} item={child} level={level + 1} />
            ))}
          </div>
        )}
      </SidebarMenuItem>
    );
  }

  // --- Leaf menu (no children) ---
  return (
    <SidebarMenuItem>
      <SidebarMenuButton asChild>
        <NavLink
          to={item.route ?? "#"}
          className={({ isActive: linkActive }) =>
            cn(
              "flex items-center w-full px-3 py-2 text-sm rounded-lg transition-colors",
              linkActive
                ? "bg-brand-primary text-white"
                : "text-foreground hover:bg-muted"
            )
          }
        >
          {({ isActive: linkActive }) => (
            <>
              <Icon
                className={cn(
                  "mr-3 h-4 w-4 shrink-0 transition-colors",
                  linkActive && "text-white"
                )}
              />
              {!isCollapsed && <span>{item.displayName}</span>}
            </>
          )}
        </NavLink>
      </SidebarMenuButton>
    </SidebarMenuItem>
  );
}

/**
 * Main Sidebar Component
 */
export function AppSidebar() {
  const { user } = useAuth();
  const { state } = useSidebar();
  const isCollapsed = state === "collapsed";
  const [menus, setMenus] = useState<MenuItem[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchMenus = async () => {
      try {
        const flatMenus = await getMenus();

        // ✅ Sort top-level menus based on sortOrder before building tree
        const sortedMenus = [...flatMenus].sort(
          (a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0)
        );

        const treeMenus = buildMenuTree(sortedMenus);
        setMenus(treeMenus);
      } catch (error) {
        console.error("Failed to load menus:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchMenus();
  }, []);

  if (!user) return null;

  return (
    <Sidebar className={cn(isCollapsed ? "w-16" : "w-64")} collapsible="icon">
      <SidebarContent>
        {/* --- Header --- */}
        <div className="p-4 border-b">
          <div className="flex items-center space-x-3">
            <div className="w-8 h-8 bg-brand-primary rounded-lg flex items-center justify-center">
              <GraduationCap className="h-5 w-5 text-white" />
            </div>
            {!isCollapsed && (
              <div>
                <h2 className="text-lg font-semibold">EduManage</h2>
                <p className="text-xs text-muted-foreground">
                  School Management
                </p>
              </div>
            )}
          </div>
        </div>

        {/* --- Navigation --- */}
        <SidebarGroup>
          <SidebarGroupLabel>Navigation</SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              {loading ? (
                <div className="text-sm text-muted-foreground px-3 py-2">
                  Loading menus...
                </div>
              ) : (
                menus.map((item) => <NavItemComponent key={item.id} item={item} />)
              )}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>

        {/* --- Footer --- */}
        {!isCollapsed && (
          <div className="mt-auto p-4 border-t">
            <div className="flex items-center space-x-3">
              <div className="w-8 h-8 bg-muted rounded-full flex items-center justify-center">
                <span className="text-sm font-medium">
                  {`${user.firstName?.[0] ?? ""}${user.lastName?.[0] ?? ""}`.toUpperCase()}
                </span>
              </div>
              <div className="flex-1 min-w-0">
                <p className="text-sm font-medium truncate">
                  {user.firstName} {user.lastName}
                </p>
                <p className="text-xs text-muted-foreground capitalize">
                  {user.roles.length > 0 ? user.roles[0] : "User"}
                </p>
              </div>
            </div>
          </div>
        )}
      </SidebarContent>
    </Sidebar>
  );
}
