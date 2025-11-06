// src/utils/menuUtils.ts
import { MenuItem } from "@/services/MenuService";

export function buildMenuTree(flatMenus: MenuItem[]): MenuItem[] {
  const menuMap = new Map<string, MenuItem>();

  // Initialize map
  flatMenus.forEach(menu => {
    menu.subMenus = [];
    menuMap.set(menu.id, menu);
  });

  const rootMenus: MenuItem[] = [];

  // Link parent-child relationships
  flatMenus.forEach(menu => {
    if (menu.parentMenuId) {
      const parent = menuMap.get(menu.parentMenuId);
      if (parent) {
        parent.subMenus!.push(menu);
      }
    } else {
      rootMenus.push(menu);
    }
  });

  // Sort menus if needed
  rootMenus.sort((a, b) => a.displayName.localeCompare(b.displayName));
  rootMenus.forEach(menu => {
    if (menu.subMenus) {
      menu.subMenus.sort((a, b) => a.displayName.localeCompare(b.displayName));
    }
  });

  return rootMenus;
}
