/**
 * Class Management Module
 * Comprehensive CRUD functionality for managing school classes
 * Features: List view, Add/Edit forms, Delete, Search, Filter
 */

import { useState, useEffect } from "react";
import { Plus, Search, Edit, Trash2, Eye, Users, ArrowUpDown, ArrowUp, ArrowDown } from "lucide-react";
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
import { classAPI, Class } from "@/services/mockData";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";

export default function ClassManagement() {
  const [classes, setClasses] = useState<Class[]>([]);
  const [filteredClasses, setFilteredClasses] = useState<Class[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [isViewDialogOpen, setIsViewDialogOpen] = useState(false);
  const [selectedClass, setSelectedClass] = useState<Class | null>(null);
  const [sortColumn, setSortColumn] = useState<string>("");
  const [sortDirection, setSortDirection] = useState<"asc" | "desc">("asc");
  const [formData, setFormData] = useState<Partial<Class>>({
    name: "",
    section: "",
    grade: "",
    classTeacher: "",
    roomNumber: "",
    capacity: 40,
    currentStrength: 0,
    subjects: [],
    schedule: "",
    academicYear: "2024-2025",
    status: "active",
  });

  useEffect(() => {
    loadClasses();
  }, []);

  useEffect(() => {
    let filtered = classes.filter(
      (cls) =>
        cls.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        cls.section.toLowerCase().includes(searchTerm.toLowerCase()) ||
        cls.classTeacher.toLowerCase().includes(searchTerm.toLowerCase())
    );

    if (sortColumn) {
      filtered.sort((a, b) => {
        let aValue: any = a[sortColumn as keyof typeof a];
        let bValue: any = b[sortColumn as keyof typeof b];
        
        if (typeof aValue === "string") aValue = aValue.toLowerCase();
        if (typeof bValue === "string") bValue = bValue.toLowerCase();
        
        if (aValue < bValue) return sortDirection === "asc" ? -1 : 1;
        if (aValue > bValue) return sortDirection === "asc" ? 1 : -1;
        return 0;
      });
    }

    setFilteredClasses(filtered);
  }, [searchTerm, classes, sortColumn, sortDirection]);

  const loadClasses = async () => {
    try {
      const data = await classAPI.getAll();
      setClasses(data);
      setFilteredClasses(data);
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to load classes",
        variant: "destructive",
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

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      if (selectedClass) {
        await classAPI.update(selectedClass.id, formData);
        toast({
          title: "Success",
          description: "Class updated successfully",
        });
      } else {
        await classAPI.create(formData as Omit<Class, "id">);
        toast({
          title: "Success",
          description: "Class created successfully",
        });
      }
      loadClasses();
      resetForm();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to save class",
        variant: "destructive",
      });
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Are you sure you want to delete this class?")) return;

    try {
      await classAPI.delete(id);
      toast({
        title: "Success",
        description: "Class deleted successfully",
      });
      loadClasses();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete class",
        variant: "destructive",
      });
    }
  };

  const handleEdit = (cls: Class) => {
    setSelectedClass(cls);
    setFormData(cls);
    setIsDialogOpen(true);
  };

  const handleView = (cls: Class) => {
    setSelectedClass(cls);
    setIsViewDialogOpen(true);
  };

  const resetForm = () => {
    setFormData({
      name: "",
      section: "",
      grade: "",
      classTeacher: "",
      roomNumber: "",
      capacity: 40,
      currentStrength: 0,
      subjects: [],
      schedule: "",
      academicYear: "2024-2025",
      status: "active",
    });
    setSelectedClass(null);
    setIsDialogOpen(false);
  };

  const handleSubjectsChange = (value: string) => {
    const subjects = value.split(',').map(s => s.trim()).filter(s => s);
    setFormData({ ...formData, subjects });
  };

  return (
    <div className="p-6 space-y-6">
      {/* Header Section */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold bg-gradient-primary bg-clip-text text-transparent">
            Class Management
          </h1>
          <p className="text-muted-foreground mt-1">
            Manage school classes, sections, and assignments
          </p>
        </div>
        <Button onClick={() => setIsDialogOpen(true)} className="btn-professional">
          <Plus className="mr-2 h-4 w-4" />
          Add New Class
        </Button>
      </div>

      {/* Search and Filter */}
      <Card className="p-4 card-gradient glow-on-hover">
        <div className="flex gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground h-4 w-4" />
            <Input
              placeholder="Search by class name, section, or teacher..."
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
              <p className="text-sm text-muted-foreground">Total Classes</p>
              <p className="text-2xl font-bold">{classes.length}</p>
            </div>
            <Users className="h-8 w-8 text-primary" />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Active Classes</p>
              <p className="text-2xl font-bold">
                {classes.filter((c) => c.status === "active").length}
              </p>
            </div>
            <Users className="h-8 w-8" style={{ color: 'hsl(var(--education-green))' }} />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Total Students</p>
              <p className="text-2xl font-bold">
                {classes.reduce((sum, c) => sum + c.currentStrength, 0)}
              </p>
            </div>
            <Users className="h-8 w-8" style={{ color: 'hsl(var(--education-blue))' }} />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Avg. Class Size</p>
              <p className="text-2xl font-bold">
                {classes.length > 0
                  ? Math.round(
                      classes.reduce((sum, c) => sum + c.currentStrength, 0) /
                        classes.length
                    )
                  : 0}
              </p>
            </div>
            <Users className="h-8 w-8" style={{ color: 'hsl(var(--education-orange))' }} />
          </div>
        </Card>
      </div>

      {/* Classes Table */}
      <Card className="card-gradient">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead 
                className="cursor-pointer select-none hover:bg-muted/50"
                onClick={() => handleSort("name")}
              >
                <div className="flex items-center">
                  Class
                  {getSortIcon("name")}
                </div>
              </TableHead>
              <TableHead 
                className="cursor-pointer select-none hover:bg-muted/50"
                onClick={() => handleSort("section")}
              >
                <div className="flex items-center">
                  Section
                  {getSortIcon("section")}
                </div>
              </TableHead>
              <TableHead 
                className="cursor-pointer select-none hover:bg-muted/50"
                onClick={() => handleSort("classTeacher")}
              >
                <div className="flex items-center">
                  Class Teacher
                  {getSortIcon("classTeacher")}
                </div>
              </TableHead>
              <TableHead 
                className="cursor-pointer select-none hover:bg-muted/50"
                onClick={() => handleSort("roomNumber")}
              >
                <div className="flex items-center">
                  Room
                  {getSortIcon("roomNumber")}
                </div>
              </TableHead>
              <TableHead 
                className="cursor-pointer select-none hover:bg-muted/50"
                onClick={() => handleSort("currentStrength")}
              >
                <div className="flex items-center">
                  Strength
                  {getSortIcon("currentStrength")}
                </div>
              </TableHead>
              <TableHead 
                className="cursor-pointer select-none hover:bg-muted/50"
                onClick={() => handleSort("status")}
              >
                <div className="flex items-center">
                  Status
                  {getSortIcon("status")}
                </div>
              </TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {filteredClasses.map((cls) => (
              <TableRow key={cls.id}>
                <TableCell className="font-medium">{cls.name}</TableCell>
                <TableCell>{cls.section}</TableCell>
                <TableCell>{cls.classTeacher}</TableCell>
                <TableCell>{cls.roomNumber}</TableCell>
                <TableCell>
                  {cls.currentStrength}/{cls.capacity}
                </TableCell>
                <TableCell>
                  <Badge variant={cls.status === "active" ? "default" : "secondary"}>
                    {cls.status}
                  </Badge>
                </TableCell>
                <TableCell className="text-right">
                  <div className="flex justify-end gap-2">
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => handleView(cls)}
                    >
                      <Eye className="h-4 w-4" />
                    </Button>
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => handleEdit(cls)}
                    >
                      <Edit className="h-4 w-4" />
                    </Button>
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => handleDelete(cls.id)}
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
        <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>
              {selectedClass ? "Edit Class" : "Add New Class"}
            </DialogTitle>
            <DialogDescription>
              {selectedClass
                ? "Update class information"
                : "Create a new class"}
            </DialogDescription>
          </DialogHeader>
          <form onSubmit={handleSubmit}>
            <div className="grid gap-4 py-4">
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label htmlFor="name">Class Name</Label>
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
                  <Label htmlFor="section">Section</Label>
                  <Input
                    id="section"
                    value={formData.section}
                    onChange={(e) =>
                      setFormData({ ...formData, section: e.target.value })
                    }
                    required
                  />
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label htmlFor="grade">Grade</Label>
                  <Input
                    id="grade"
                    value={formData.grade}
                    onChange={(e) =>
                      setFormData({ ...formData, grade: e.target.value })
                    }
                    required
                  />
                </div>
                <div>
                  <Label htmlFor="roomNumber">Room Number</Label>
                  <Input
                    id="roomNumber"
                    value={formData.roomNumber}
                    onChange={(e) =>
                      setFormData({ ...formData, roomNumber: e.target.value })
                    }
                    required
                  />
                </div>
              </div>

              <div>
                <Label htmlFor="classTeacher">Class Teacher</Label>
                <Input
                  id="classTeacher"
                  value={formData.classTeacher}
                  onChange={(e) =>
                    setFormData({ ...formData, classTeacher: e.target.value })
                  }
                  required
                />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label htmlFor="capacity">Capacity</Label>
                  <Input
                    id="capacity"
                    type="number"
                    value={formData.capacity}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        capacity: parseInt(e.target.value),
                      })
                    }
                    required
                  />
                </div>
                <div>
                  <Label htmlFor="currentStrength">Current Strength</Label>
                  <Input
                    id="currentStrength"
                    type="number"
                    value={formData.currentStrength}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        currentStrength: parseInt(e.target.value),
                      })
                    }
                    required
                  />
                </div>
              </div>

              <div>
                <Label htmlFor="subjects">Subjects (comma-separated)</Label>
                <Input
                  id="subjects"
                  value={formData.subjects?.join(', ')}
                  onChange={(e) => handleSubjectsChange(e.target.value)}
                  placeholder="Mathematics, Science, English"
                />
              </div>

              <div>
                <Label htmlFor="schedule">Schedule</Label>
                <Input
                  id="schedule"
                  value={formData.schedule}
                  onChange={(e) =>
                    setFormData({ ...formData, schedule: e.target.value })
                  }
                  placeholder="Monday-Friday, 8:00 AM - 2:00 PM"
                />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label htmlFor="academicYear">Academic Year</Label>
                  <Input
                    id="academicYear"
                    value={formData.academicYear}
                    onChange={(e) =>
                      setFormData({ ...formData, academicYear: e.target.value })
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
                {selectedClass ? "Update" : "Create"}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>

      {/* View Dialog */}
      <Dialog open={isViewDialogOpen} onOpenChange={setIsViewDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Class Details</DialogTitle>
          </DialogHeader>
          {selectedClass && (
            <div className="space-y-4">
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label className="text-muted-foreground">Class Name</Label>
                  <p className="font-medium">{selectedClass.name}</p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Section</Label>
                  <p className="font-medium">{selectedClass.section}</p>
                </div>
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label className="text-muted-foreground">Grade</Label>
                  <p className="font-medium">{selectedClass.grade}</p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Room Number</Label>
                  <p className="font-medium">{selectedClass.roomNumber}</p>
                </div>
              </div>
              <div>
                <Label className="text-muted-foreground">Class Teacher</Label>
                <p className="font-medium">{selectedClass.classTeacher}</p>
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label className="text-muted-foreground">Capacity</Label>
                  <p className="font-medium">{selectedClass.capacity}</p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Current Strength</Label>
                  <p className="font-medium">{selectedClass.currentStrength}</p>
                </div>
              </div>
              <div>
                <Label className="text-muted-foreground">Subjects</Label>
                <div className="flex flex-wrap gap-2 mt-1">
                  {selectedClass.subjects.map((subject, index) => (
                    <Badge key={index} variant="secondary">
                      {subject}
                    </Badge>
                  ))}
                </div>
              </div>
              <div>
                <Label className="text-muted-foreground">Schedule</Label>
                <p className="font-medium">{selectedClass.schedule}</p>
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label className="text-muted-foreground">Academic Year</Label>
                  <p className="font-medium">{selectedClass.academicYear}</p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Status</Label>
                  <Badge variant={selectedClass.status === "active" ? "default" : "secondary"}>
                    {selectedClass.status}
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
