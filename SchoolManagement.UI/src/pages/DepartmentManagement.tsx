/**
 * Department Management Module
 * Comprehensive CRUD functionality for managing school departments
 * Features: List view, Add/Edit forms, Delete, Search, Budget tracking
 */

import { useState, useEffect } from "react";
import { Plus, Search, Edit, Trash2, Eye, Building2, Users, DollarSign } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Label } from "@/components/ui/label";
import { toast } from "@/hooks/use-toast";
import { departmentAPI, Department } from "@/services/mockData";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";

export default function DepartmentManagement() {
  const [departments, setDepartments] = useState<Department[]>([]);
  const [filteredDepartments, setFilteredDepartments] = useState<Department[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [isViewDialogOpen, setIsViewDialogOpen] = useState(false);
  const [selectedDepartment, setSelectedDepartment] = useState<Department | null>(null);
  const [formData, setFormData] = useState<Partial<Department>>({
    name: "",
    code: "",
    head: "",
    description: "",
    employeeCount: 0,
    budget: 0,
    establishedDate: "",
    status: "active",
  });

  useEffect(() => {
    loadDepartments();
  }, []);

  useEffect(() => {
    const filtered = departments.filter(
      (dept) =>
        dept.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        dept.code.toLowerCase().includes(searchTerm.toLowerCase()) ||
        dept.head.toLowerCase().includes(searchTerm.toLowerCase())
    );
    setFilteredDepartments(filtered);
  }, [searchTerm, departments]);

  const loadDepartments = async () => {
    try {
      const data = await departmentAPI.getAll();
      setDepartments(data);
      setFilteredDepartments(data);
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to load departments",
        variant: "destructive",
      });
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      if (selectedDepartment) {
        await departmentAPI.update(selectedDepartment.id, formData);
        toast({
          title: "Success",
          description: "Department updated successfully",
        });
      } else {
        await departmentAPI.create(formData as Omit<Department, "id">);
        toast({
          title: "Success",
          description: "Department created successfully",
        });
      }
      loadDepartments();
      resetForm();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to save department",
        variant: "destructive",
      });
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Are you sure you want to delete this department?")) return;

    try {
      await departmentAPI.delete(id);
      toast({
        title: "Success",
        description: "Department deleted successfully",
      });
      loadDepartments();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete department",
        variant: "destructive",
      });
    }
  };

  const handleEdit = (dept: Department) => {
    setSelectedDepartment(dept);
    setFormData(dept);
    setIsDialogOpen(true);
  };

  const handleView = (dept: Department) => {
    setSelectedDepartment(dept);
    setIsViewDialogOpen(true);
  };

  const resetForm = () => {
    setFormData({
      name: "",
      code: "",
      head: "",
      description: "",
      employeeCount: 0,
      budget: 0,
      establishedDate: "",
      status: "active",
    });
    setSelectedDepartment(null);
    setIsDialogOpen(false);
  };

  const totalBudget = departments.reduce((sum, dept) => sum + dept.budget, 0);
  const totalEmployees = departments.reduce((sum, dept) => sum + dept.employeeCount, 0);

  return (
    <div className="p-6 space-y-6">
      {/* Header Section */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold bg-gradient-primary bg-clip-text text-transparent">
            Department Management
          </h1>
          <p className="text-muted-foreground mt-1">
            Manage school departments, heads, and budgets
          </p>
        </div>
        <Button onClick={() => setIsDialogOpen(true)} className="btn-professional">
          <Plus className="mr-2 h-4 w-4" />
          Add New Department
        </Button>
      </div>

      {/* Search and Filter */}
      <Card className="p-4 card-gradient glow-on-hover">
        <div className="flex gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground h-4 w-4" />
            <Input
              placeholder="Search by name, code, or department head..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="pl-10"
            />
          </div>
        </div>
      </Card>

      {/* Statistics Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Total Departments</p>
              <p className="text-2xl font-bold">{departments.length}</p>
            </div>
            <Building2 className="h-8 w-8 text-primary" />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Active Departments</p>
              <p className="text-2xl font-bold">
                {departments.filter((d) => d.status === "active").length}
              </p>
            </div>
            <Building2 className="h-8 w-8" style={{ color: 'hsl(var(--education-green))' }} />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Total Employees</p>
              <p className="text-2xl font-bold">{totalEmployees}</p>
            </div>
            <Users className="h-8 w-8" style={{ color: 'hsl(var(--education-blue))' }} />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Total Budget</p>
              <p className="text-2xl font-bold">${(totalBudget / 1000).toFixed(0)}K</p>
            </div>
            <DollarSign className="h-8 w-8" style={{ color: 'hsl(var(--education-orange))' }} />
          </div>
        </Card>
      </div>

      {/* Departments Table */}
      <Card className="card-gradient">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Department</TableHead>
              <TableHead>Code</TableHead>
              <TableHead>Head</TableHead>
              <TableHead>Employees</TableHead>
              <TableHead>Budget</TableHead>
              <TableHead>Status</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {filteredDepartments.map((dept) => (
              <TableRow key={dept.id}>
                <TableCell className="font-medium">{dept.name}</TableCell>
                <TableCell>
                  <Badge variant="outline">{dept.code}</Badge>
                </TableCell>
                <TableCell>{dept.head}</TableCell>
                <TableCell>{dept.employeeCount}</TableCell>
                <TableCell>${dept.budget.toLocaleString()}</TableCell>
                <TableCell>
                  <Badge variant={dept.status === "active" ? "default" : "secondary"}>
                    {dept.status}
                  </Badge>
                </TableCell>
                <TableCell className="text-right">
                  <div className="flex justify-end gap-2">
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => handleView(dept)}
                    >
                      <Eye className="h-4 w-4" />
                    </Button>
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => handleEdit(dept)}
                    >
                      <Edit className="h-4 w-4" />
                    </Button>
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => handleDelete(dept.id)}
                    >
                      <Trash2 className="h-4 w-4 text-destructive" />
                    </Button>
                  </div>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Card>

      {/* Add/Edit Dialog */}
      <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
        <DialogContent className="max-w-2xl">
          <DialogHeader>
            <DialogTitle>
              {selectedDepartment ? "Edit Department" : "Add New Department"}
            </DialogTitle>
            <DialogDescription>
              {selectedDepartment
                ? "Update department information"
                : "Create a new department"}
            </DialogDescription>
          </DialogHeader>
          <form onSubmit={handleSubmit}>
            <div className="grid gap-4 py-4">
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label htmlFor="name">Department Name</Label>
                  <Input
                    id="name"
                    value={formData.name}
                    onChange={(e) =>
                      setFormData({ ...formData, name: e.target.value })
                    }
                    required
                  />
                </div>
                <div>
                  <Label htmlFor="code">Department Code</Label>
                  <Input
                    id="code"
                    value={formData.code}
                    onChange={(e) =>
                      setFormData({ ...formData, code: e.target.value.toUpperCase() })
                    }
                    required
                  />
                </div>
              </div>

              <div>
                <Label htmlFor="head">Department Head</Label>
                <Input
                  id="head"
                  value={formData.head}
                  onChange={(e) =>
                    setFormData({ ...formData, head: e.target.value })
                  }
                  required
                />
              </div>

              <div>
                <Label htmlFor="description">Description</Label>
                <Textarea
                  id="description"
                  value={formData.description}
                  onChange={(e) =>
                    setFormData({ ...formData, description: e.target.value })
                  }
                  rows={3}
                />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label htmlFor="employeeCount">Employee Count</Label>
                  <Input
                    id="employeeCount"
                    type="number"
                    value={formData.employeeCount}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        employeeCount: parseInt(e.target.value) || 0,
                      })
                    }
                    required
                  />
                </div>
                <div>
                  <Label htmlFor="budget">Budget ($)</Label>
                  <Input
                    id="budget"
                    type="number"
                    value={formData.budget}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        budget: parseInt(e.target.value) || 0,
                      })
                    }
                    required
                  />
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label htmlFor="establishedDate">Established Date</Label>
                  <Input
                    id="establishedDate"
                    type="date"
                    value={formData.establishedDate}
                    onChange={(e) =>
                      setFormData({ ...formData, establishedDate: e.target.value })
                    }
                    required
                  />
                </div>
                <div>
                  <Label htmlFor="status">Status</Label>
                  <Select
                    value={formData.status}
                    onValueChange={(value: "active" | "inactive") =>
                      setFormData({ ...formData, status: value })
                    }
                  >
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="active">Active</SelectItem>
                      <SelectItem value="inactive">Inactive</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>
            </div>
            <DialogFooter>
              <Button type="button" variant="outline" onClick={resetForm}>
                Cancel
              </Button>
              <Button type="submit" className="btn-professional">
                {selectedDepartment ? "Update" : "Create"}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>

      {/* View Dialog */}
      <Dialog open={isViewDialogOpen} onOpenChange={setIsViewDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Department Details</DialogTitle>
          </DialogHeader>
          {selectedDepartment && (
            <div className="space-y-4">
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label className="text-muted-foreground">Department Name</Label>
                  <p className="font-medium">{selectedDepartment.name}</p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Code</Label>
                  <Badge variant="outline">{selectedDepartment.code}</Badge>
                </div>
              </div>
              <div>
                <Label className="text-muted-foreground">Department Head</Label>
                <p className="font-medium">{selectedDepartment.head}</p>
              </div>
              <div>
                <Label className="text-muted-foreground">Description</Label>
                <p className="text-sm">{selectedDepartment.description}</p>
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label className="text-muted-foreground">Employee Count</Label>
                  <p className="font-medium">{selectedDepartment.employeeCount}</p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Budget</Label>
                  <p className="font-medium">${selectedDepartment.budget.toLocaleString()}</p>
                </div>
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label className="text-muted-foreground">Established Date</Label>
                  <p className="font-medium">
                    {new Date(selectedDepartment.establishedDate).toLocaleDateString()}
                  </p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Status</Label>
                  <Badge variant={selectedDepartment.status === "active" ? "default" : "secondary"}>
                    {selectedDepartment.status}
                  </Badge>
                </div>
              </div>
            </div>
          )}
        </DialogContent>
      </Dialog>
    </div>
  );
}
