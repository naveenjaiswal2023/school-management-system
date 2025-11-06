import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Checkbox } from "@/components/ui/checkbox";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { useToast } from "@/hooks/use-toast";
import { Shield, Plus, Pencil, Trash2, Search, ArrowUpDown, ArrowUp, ArrowDown } from "lucide-react";
import { roleAPI, Role } from "@/services/mockData";

const availablePermissions = [
  { id: 'users.create', name: 'Create Users', category: 'User Management' },
  { id: 'users.read', name: 'View Users', category: 'User Management' },
  { id: 'users.update', name: 'Update Users', category: 'User Management' },
  { id: 'users.delete', name: 'Delete Users', category: 'User Management' },
  { id: 'students.create', name: 'Create Students', category: 'Student Management' },
  { id: 'students.read', name: 'View Students', category: 'Student Management' },
  { id: 'students.update', name: 'Update Students', category: 'Student Management' },
  { id: 'students.delete', name: 'Delete Students', category: 'Student Management' },
  { id: 'students.all', name: 'All Student Operations', category: 'Student Management' },
  { id: 'employees.create', name: 'Create Employees', category: 'Employee Management' },
  { id: 'employees.read', name: 'View Employees', category: 'Employee Management' },
  { id: 'employees.update', name: 'Update Employees', category: 'Employee Management' },
  { id: 'employees.delete', name: 'Delete Employees', category: 'Employee Management' },
  { id: 'employees.all', name: 'All Employee Operations', category: 'Employee Management' },
  { id: 'attendance.mark', name: 'Mark Attendance', category: 'Attendance' },
  { id: 'attendance.view', name: 'View Attendance', category: 'Attendance' },
  { id: 'exams.create', name: 'Create Exams', category: 'Examinations' },
  { id: 'exams.manage', name: 'Manage Exams', category: 'Examinations' },
  { id: 'exams.results', name: 'Manage Results', category: 'Examinations' },
  { id: 'fees.manage', name: 'Manage Fees', category: 'Fee Management' },
  { id: 'fees.collect', name: 'Collect Fees', category: 'Fee Management' },
  { id: 'payroll.manage', name: 'Manage Payroll', category: 'HRMS' },
  { id: 'reports.view', name: 'View Reports', category: 'Reports' },
  { id: 'settings.manage', name: 'Manage Settings', category: 'System' },
];

export default function RoleManagement() {
  const [roles, setRoles] = useState<Role[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [editingRole, setEditingRole] = useState<Role | null>(null);
  const [sortColumn, setSortColumn] = useState<string>("");
  const [sortDirection, setSortDirection] = useState<"asc" | "desc">("asc");
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    permissions: [] as string[]
  });
  const { toast } = useToast();

  useEffect(() => {
    loadRoles();
  }, []);

  const loadRoles = async () => {
    try {
      const data = await roleAPI.getAll();
      setRoles(data);
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to load roles",
        variant: "destructive",
      });
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingRole) {
        await roleAPI.update(editingRole.id, formData);
        toast({
          title: "Success",
          description: "Role updated successfully",
        });
      } else {
        await roleAPI.create({
          ...formData,
          createdAt: new Date().toISOString(),
        });
        toast({
          title: "Success",
          description: "Role created successfully",
        });
      }
      loadRoles();
      handleCloseDialog();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to save role",
        variant: "destructive",
      });
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await roleAPI.delete(id);
      toast({
        title: "Success",
        description: "Role deleted successfully",
      });
      loadRoles();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete role",
        variant: "destructive",
      });
    }
  };

  const handleEdit = (role: Role) => {
    setEditingRole(role);
    setFormData({
      name: role.name,
      description: role.description,
      permissions: role.permissions,
    });
    setIsDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setIsDialogOpen(false);
    setEditingRole(null);
    setFormData({
      name: "",
      description: "",
      permissions: []
    });
  };

  const handlePermissionChange = (permissionId: string, checked: boolean) => {
    if (checked) {
      setFormData({
        ...formData,
        permissions: [...formData.permissions, permissionId]
      });
    } else {
      setFormData({
        ...formData,
        permissions: formData.permissions.filter(p => p !== permissionId)
      });
    }
  };

  const handleSort = (column: string) => {
    if (sortColumn === column) {
      setSortDirection(sortDirection === "asc" ? "desc" : "asc");
    } else {
      setSortColumn(column);
      setSortDirection("asc");
    }
  };

  const getSortIcon = (column: string) => {
    if (sortColumn !== column) return <ArrowUpDown className="h-4 w-4 ml-1" />;
    return sortDirection === "asc" ? <ArrowUp className="h-4 w-4 ml-1" /> : <ArrowDown className="h-4 w-4 ml-1" />;
  };

  const filteredRoles = roles
    .filter(role =>
      role.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      role.description.toLowerCase().includes(searchTerm.toLowerCase())
    )
    .sort((a, b) => {
      if (!sortColumn) return 0;
      
      let aValue: any = a[sortColumn as keyof typeof a];
      let bValue: any = b[sortColumn as keyof typeof b];
      
      if (typeof aValue === "string") aValue = aValue.toLowerCase();
      if (typeof bValue === "string") bValue = bValue.toLowerCase();
      
      if (aValue < bValue) return sortDirection === "asc" ? -1 : 1;
      if (aValue > bValue) return sortDirection === "asc" ? 1 : -1;
      return 0;
    });

  const groupedPermissions = availablePermissions.reduce((groups, permission) => {
    if (!groups[permission.category]) {
      groups[permission.category] = [];
    }
    groups[permission.category].push(permission);
    return groups;
  }, {} as Record<string, typeof availablePermissions>);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-foreground">Role Management</h1>
          <p className="text-muted-foreground">Manage roles and permissions</p>
        </div>
        
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button className="bg-gradient-primary">
              <Plus className="mr-2 h-4 w-4" />
              Add Role
            </Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-[600px] max-h-[80vh] overflow-y-auto">
            <DialogHeader>
              <DialogTitle>
                {editingRole ? 'Edit Role' : 'Add New Role'}
              </DialogTitle>
            </DialogHeader>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="name">Role Name</Label>
                <Input
                  id="name"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  required
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="description">Description</Label>
                <Textarea
                  id="description"
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  required
                />
              </div>
              <div className="space-y-4">
                <Label>Permissions</Label>
                <div className="space-y-4 max-h-64 overflow-y-auto border rounded-lg p-4">
                  {Object.entries(groupedPermissions).map(([category, permissions]) => (
                    <div key={category} className="space-y-2">
                      <h4 className="font-medium text-sm text-muted-foreground uppercase tracking-wide">
                        {category}
                      </h4>
                      <div className="grid grid-cols-1 gap-2">
                        {permissions.map((permission) => (
                          <div key={permission.id} className="flex items-center space-x-2">
                            <Checkbox
                              id={permission.id}
                              checked={formData.permissions.includes(permission.id)}
                              onCheckedChange={(checked) => 
                                handlePermissionChange(permission.id, checked as boolean)
                              }
                            />
                            <Label htmlFor={permission.id} className="text-sm">
                              {permission.name}
                            </Label>
                          </div>
                        ))}
                      </div>
                    </div>
                  ))}
                </div>
              </div>
              <div className="flex justify-end space-x-2">
                <Button type="button" variant="outline" onClick={handleCloseDialog}>
                  Cancel
                </Button>
                <Button type="submit" className="bg-gradient-primary">
                  {editingRole ? 'Update' : 'Create'}
                </Button>
              </div>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      {/* Stats Cards */}
      <div className="grid gap-4 md:grid-cols-3">
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Total Roles
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{roles.length}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Total Permissions
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{availablePermissions.length}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Permission Categories
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{Object.keys(groupedPermissions).length}</div>
          </CardContent>
        </Card>
      </div>

      {/* Roles Table */}
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle className="flex items-center gap-2">
              <Shield className="h-5 w-5" />
              Roles List
            </CardTitle>
            <div className="flex items-center space-x-2">
              <Search className="h-4 w-4 text-muted-foreground" />
              <Input
                placeholder="Search roles..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="w-64"
              />
            </div>
          </div>
        </CardHeader>
        <CardContent>
          <div className="rounded-md border">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("name")}
                  >
                    <div className="flex items-center">
                      Role Name
                      {getSortIcon("name")}
                    </div>
                  </TableHead>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("description")}
                  >
                    <div className="flex items-center">
                      Description
                      {getSortIcon("description")}
                    </div>
                  </TableHead>
                  <TableHead>Permissions</TableHead>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("createdAt")}
                  >
                    <div className="flex items-center">
                      Created
                      {getSortIcon("createdAt")}
                    </div>
                  </TableHead>
                  <TableHead>Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredRoles.map((role) => (
                  <TableRow key={role.id}>
                    <TableCell className="font-medium">{role.name}</TableCell>
                    <TableCell>{role.description}</TableCell>
                    <TableCell>
                      <div className="flex flex-wrap gap-1">
                        {role.permissions.slice(0, 3).map((permission) => (
                          <Badge key={permission} variant="outline" className="text-xs">
                            {permission}
                          </Badge>
                        ))}
                        {role.permissions.length > 3 && (
                          <Badge variant="outline" className="text-xs">
                            +{role.permissions.length - 3} more
                          </Badge>
                        )}
                      </div>
                    </TableCell>
                    <TableCell>
                      {new Date(role.createdAt).toLocaleDateString()}
                    </TableCell>
                    <TableCell>
                      <div className="flex items-center space-x-2">
                        <Button
                          variant="outline"
                          size="sm"
                          onClick={() => handleEdit(role)}
                        >
                          <Pencil className="h-3 w-3" />
                        </Button>
                        <Button
                          variant="outline"
                          size="sm"
                          onClick={() => handleDelete(role.id)}
                          className="hover:bg-destructive hover:text-destructive-foreground"
                        >
                          <Trash2 className="h-3 w-3" />
                        </Button>
                      </div>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}