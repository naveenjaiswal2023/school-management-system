import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";
import { AppSidebar } from "./Sidebar";
import { useAuth } from "@/contexts/AuthContext";
import { Bell } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

export function Layout({ children }: { children: React.ReactNode }) {
  const { user, logout } = useAuth();

  if (!user) return null;

  // Safely get first role or default to "User"
  const primaryRole = user.roles.length > 0 ? user.roles[0] : "User";

  return (
    <SidebarProvider>
      <div className="flex min-h-screen w-full bg-gradient-to-br from-background via-muted/30 to-brand-primary/5">
        <AppSidebar />
        
        <div className="flex-1 flex flex-col">
          {/* Enhanced Header with gradient */}
          <header className="h-16 bg-gradient-to-r from-card/95 to-background/95 backdrop-blur-sm border-b border-border/50 flex items-center justify-between px-6 relative">
            {/* Subtle glow effect */}
            <div className="absolute inset-0 bg-gradient-to-r from-brand-primary/5 via-transparent to-brand-accent/5 pointer-events-none" />
            
            <div className="flex items-center space-x-4 relative z-10">
              <SidebarTrigger className="glow-on-hover" />
              <div>
                <h1 className="text-xl font-bold bg-gradient-primary bg-clip-text text-transparent">
                  Welcome back, {user.firstName}
                </h1>
                <p className="text-sm text-muted-foreground capitalize font-medium">
                  {primaryRole} Dashboard
                </p>
              </div>
            </div>

            <div className="flex items-center space-x-3 relative z-10">
              <Button variant="ghost" size="sm" className="glow-on-hover relative">
                <Bell className="h-4 w-4" />
                {/* Notification badge */}
                <span className="absolute -top-1 -right-1 h-3 w-3 bg-education-orange rounded-full border-2 border-background animate-pulse-glow" />
              </Button>
              
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button variant="ghost" size="sm" className="glow-on-hover">
                    <div className="h-8 w-8 rounded-full bg-gradient-primary flex items-center justify-center text-white font-semibold">
                      {user.firstName.charAt(0)}
                    </div>
                  </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end" className="glass">
                  <DropdownMenuItem className="font-medium">{user.firstName} {user.lastName}</DropdownMenuItem>
                  <DropdownMenuItem className="text-muted-foreground">{user.email}</DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem className="hover:bg-brand-primary/10">Profile</DropdownMenuItem>
                  <DropdownMenuItem className="hover:bg-brand-primary/10">Settings</DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem onClick={logout} className="text-destructive hover:bg-destructive/10">
                    Logout
                  </DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            </div>
          </header>

          {/* Enhanced Main Content */}
          <main className="flex-1 p-6 relative">
            {/* Subtle background pattern */}
            <div className="absolute inset-0 opacity-30 bg-[radial-gradient(circle_at_50%_50%,_hsl(var(--brand-primary)/0.05)_0%,_transparent_50%)]" />
            <div className="relative z-10">
              {children}
            </div>
          </main>
        </div>
      </div>
    </SidebarProvider>
  );
}
