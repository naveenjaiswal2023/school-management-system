import { apiFetch } from "@/lib/apiFetch";

export interface MenuItem {
  id: string;
  name: string;
  displayName: string;
  description?: string;
  icon?: string;
  route?: string;
  parentMenuId?: string | null; // original from API
  parentId?: string | null;     // normalized for UI utils
  sortOrder?: number;           // API field for order
  order?: number;               // normalized field for sorting
  displayOrder?: number;        // sometimes APIs use this
  subMenus?: MenuItem[];
}

/**
 * Fetches menus from the API and normalizes field names
 * to ensure consistent usage across the app (e.g., parentId, order).
 */
export async function getMenus(): Promise<MenuItem[]> {
  const response = await apiFetch("/Menus/hierarchy");

  if (!Array.isArray(response)) {
    console.error("❌ Invalid API response for menus:", response);
    return [];
  }

  // Normalize and safely map each item
  const normalized: MenuItem[] = response.map((item: any): MenuItem => ({
    id: item.id,
    name: item.name,
    displayName: item.displayName,
    description: item.description,
    icon: item.icon,
    route: item.route,
    parentMenuId: item.parentMenuId ?? null,
    parentId: item.parentMenuId ?? null,
    sortOrder: item.sortOrder ?? item.displayOrder ?? item.order ?? 0,
    order: item.sortOrder ?? item.displayOrder ?? item.order ?? 0,
    subMenus: Array.isArray(item.subMenus) ? item.subMenus : [],
  }));

  // Sort by sortOrder (with fallback to displayName if needed)
  normalized.sort((a, b) => {
    const orderA = a.order ?? 0;
    const orderB = b.order ?? 0;
    if (orderA === orderB) {
      return a.displayName.localeCompare(b.displayName);
    }
    return orderA - orderB;
  });

  return normalized;
}
