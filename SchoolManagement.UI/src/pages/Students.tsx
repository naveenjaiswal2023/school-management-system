import { useState } from "react";
import { useAuth } from "@/contexts/AuthContext";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Search, Plus, Eye, Edit, Trash2, ArrowUpDown, ArrowUp, ArrowDown } from "lucide-react";
import { StudentDialog } from "@/components/students/StudentDialog";
import { StudentDetailsDialog } from "@/components/students/StudentDetailsDialog";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog";
import { toast } from "@/hooks/use-toast";

interface Student {
  id: string;
  name: string;
  email: string;
  class: string;
  rollNumber: string;
  attendance: number;
  status: string;
  phone: string;
  parent: string;
}

const initialStudents: Student[] = [
  {
    id: "1",
    name: "Alice Johnson",
    email: "alice.johnson@student.edu",
    class: "10A",
    rollNumber: "2024001",
    attendance: 94,
    status: "Active",
    phone: "+1234567890",
    parent: "Robert Johnson",
  },
  {
    id: "2",
    name: "Bob Smith",
    email: "bob.smith@student.edu",
    class: "10B",
    rollNumber: "2024002",
    attendance: 87,
    status: "Active",
    phone: "+1234567891",
    parent: "Mary Smith",
  },
  {
    id: "3",
    name: "Carol Davis",
    email: "carol.davis@student.edu",
    class: "9A",
    rollNumber: "2024003",
    attendance: 91,
    status: "Active",
    phone: "+1234567892",
    parent: "James Davis",
  },
  {
    id: "4",
    name: "David Wilson",
    email: "david.wilson@student.edu",
    class: "11A",
    rollNumber: "2024004",
    attendance: 76,
    status: "Warning",
    phone: "+1234567893",
    parent: "Lisa Wilson",
  },
  {
    id: "5",
    name: "Emma Brown",
    email: "emma.brown@student.edu",
    class: "12A",
    rollNumber: "2024005",
    attendance: 98,
    status: "Active",
    phone: "+1234567894",
    parent: "Michael Brown",
  },
];

export default function Students() {
  const { user } = useAuth();
  const [students, setStudents] = useState<Student[]>(initialStudents);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedClass, setSelectedClass] = useState("all");
  const [sortColumn, setSortColumn] = useState<string>("");
  const [sortDirection, setSortDirection] = useState<"asc" | "desc">("asc");
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [isDetailsOpen, setIsDetailsOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);

  if (!user || !["admin", "teacher"].includes(user.role)) {
    return (
      <div className="flex items-center justify-center h-64">
        <p className="text-muted-foreground">Access denied. This page is only available to administrators and teachers.</p>
      </div>
    );
  }

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

  const filteredStudents = students
    .filter((student) => {
      const matchesSearch = student.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                           student.rollNumber.toLowerCase().includes(searchTerm.toLowerCase()) ||
                           student.email.toLowerCase().includes(searchTerm.toLowerCase());
      const matchesClass = selectedClass === "all" || student.class === selectedClass;
      return matchesSearch && matchesClass;
    })
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

  const getStatusBadge = (status: string, attendance: number) => {
    if (status === "Warning" || attendance < 80) {
      return <Badge variant="destructive">Warning</Badge>;
    }
    return <Badge className="bg-education-green text-white">Active</Badge>;
  };

  const getAttendanceColor = (attendance: number) => {
    if (attendance >= 90) return "text-education-green";
    if (attendance >= 75) return "text-education-orange";
    return "text-destructive";
  };

  const handleAddStudent = () => {
    setSelectedStudent(null);
    setIsDialogOpen(true);
  };

  const handleEditStudent = (student: Student) => {
    setSelectedStudent(student);
    setIsDialogOpen(true);
  };

  const handleViewStudent = (student: Student) => {
    setSelectedStudent(student);
    setIsDetailsOpen(true);
  };

  const handleDeleteClick = (student: Student) => {
    setSelectedStudent(student);
    setIsDeleteDialogOpen(true);
  };

  const handleDeleteConfirm = () => {
    if (selectedStudent) {
      setStudents(students.filter((s) => s.id !== selectedStudent.id));
      toast({
        title: "Student deleted",
        description: `${selectedStudent.name} has been removed from the system.`,
      });
      setIsDeleteDialogOpen(false);
      setSelectedStudent(null);
    }
  };

  const handleSaveStudent = (student: Student) => {
    if (selectedStudent) {
      setStudents(students.map((s) => (s.id === student.id ? student : s)));
      toast({
        title: "Student updated",
        description: `${student.name}'s information has been updated.`,
      });
    } else {
      setStudents([...students, student]);
      toast({
        title: "Student added",
        description: `${student.name} has been registered successfully.`,
      });
    }
    setIsDialogOpen(false);
    setSelectedStudent(null);
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-foreground">Students</h1>
          <p className="text-muted-foreground">Manage student information and records</p>
        </div>
        {user.role === "admin" && (
          <Button className="bg-gradient-primary" onClick={handleAddStudent}>
            <Plus className="mr-2 h-4 w-4" />
            Add Student
          </Button>
        )}
      </div>

      {/* Stats Cards */}
      <div className="grid gap-4 md:grid-cols-4">
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Total Students
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">1,248</div>
            <p className="text-xs text-muted-foreground">+12% from last month</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Active Students
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">1,195</div>
            <p className="text-xs text-muted-foreground">95.8% active rate</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Average Attendance
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">89.2%</div>
            <p className="text-xs text-muted-foreground">+2.1% from last month</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              New Admissions
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">53</div>
            <p className="text-xs text-muted-foreground">This month</p>
          </CardContent>
        </Card>
      </div>

      {/* Filters */}
      <Card>
        <CardContent className="pt-6">
          <div className="flex gap-4 mb-4">
            <div className="flex-1">
              <div className="relative">
                <Search className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                <Input
                  placeholder="Search students by name, roll number, or email..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="pl-10"
                />
              </div>
            </div>
            <select
              value={selectedClass}
              onChange={(e) => setSelectedClass(e.target.value)}
              className="px-3 py-2 border rounded-md bg-background text-foreground"
            >
              <option value="all">All Classes</option>
              <option value="9A">Class 9A</option>
              <option value="10A">Class 10A</option>
              <option value="10B">Class 10B</option>
              <option value="11A">Class 11A</option>
              <option value="12A">Class 12A</option>
            </select>
          </div>

          {/* Students Table */}
          <div className="rounded-md border">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("name")}
                  >
                    <div className="flex items-center">
                      Student
                      {getSortIcon("name")}
                    </div>
                  </TableHead>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("rollNumber")}
                  >
                    <div className="flex items-center">
                      Roll Number
                      {getSortIcon("rollNumber")}
                    </div>
                  </TableHead>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("class")}
                  >
                    <div className="flex items-center">
                      Class
                      {getSortIcon("class")}
                    </div>
                  </TableHead>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("attendance")}
                  >
                    <div className="flex items-center">
                      Attendance
                      {getSortIcon("attendance")}
                    </div>
                  </TableHead>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("parent")}
                  >
                    <div className="flex items-center">
                      Parent/Guardian
                      {getSortIcon("parent")}
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
                  <TableHead>Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredStudents.map((student) => (
                  <TableRow key={student.id}>
                    <TableCell>
                      <div className="flex items-center space-x-3">
                        <Avatar>
                          <AvatarFallback>
                            {student.name.split(' ').map(n => n[0]).join('')}
                          </AvatarFallback>
                        </Avatar>
                        <div>
                          <div className="font-medium">{student.name}</div>
                          <div className="text-sm text-muted-foreground">{student.email}</div>
                        </div>
                      </div>
                    </TableCell>
                    <TableCell className="font-mono">{student.rollNumber}</TableCell>
                    <TableCell>{student.class}</TableCell>
                    <TableCell>
                      <span className={`font-medium ${getAttendanceColor(student.attendance)}`}>
                        {student.attendance}%
                      </span>
                    </TableCell>
                    <TableCell>{student.parent}</TableCell>
                    <TableCell>
                      {getStatusBadge(student.status, student.attendance)}
                    </TableCell>
                    <TableCell>
                      <div className="flex items-center space-x-2">
                        <Button variant="ghost" size="sm" onClick={() => handleViewStudent(student)}>
                          <Eye className="h-4 w-4" />
                        </Button>
                        {user.role === "admin" && (
                          <>
                            <Button variant="ghost" size="sm" onClick={() => handleEditStudent(student)}>
                              <Edit className="h-4 w-4" />
                            </Button>
                            <Button variant="ghost" size="sm" onClick={() => handleDeleteClick(student)}>
                              <Trash2 className="h-4 w-4 text-destructive" />
                            </Button>
                          </>
                        )}
                      </div>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>

          {filteredStudents.length === 0 && (
            <div className="text-center py-12">
              <p className="text-muted-foreground">No students found matching your criteria.</p>
            </div>
          )}
        </CardContent>
      </Card>

      <StudentDialog
        open={isDialogOpen}
        onOpenChange={setIsDialogOpen}
        student={selectedStudent}
        onSave={handleSaveStudent}
      />

      <StudentDetailsDialog
        open={isDetailsOpen}
        onOpenChange={setIsDetailsOpen}
        student={selectedStudent}
      />

      <AlertDialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete Student</AlertDialogTitle>
            <AlertDialogDescription>
              Are you sure you want to delete {selectedStudent?.name}? This action cannot be undone.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction onClick={handleDeleteConfirm} className="bg-destructive text-destructive-foreground hover:bg-destructive/90">
              Delete
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
}