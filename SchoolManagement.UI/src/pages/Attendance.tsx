import { useState } from "react";
import { useAuth } from "@/contexts/AuthContext";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Calendar } from "@/components/ui/calendar";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Check, X, Clock, Users, UserCheck, Calendar as CalendarIcon, ArrowUpDown, ArrowUp, ArrowDown } from "lucide-react";
import { format } from "date-fns";

// Sample attendance data
const attendanceRecords = [
  {
    id: "1",
    studentName: "Alice Johnson",
    rollNumber: "2024001",
    class: "10A",
    status: "present",
    time: "08:30 AM",
    date: "2024-12-27",
  },
  {
    id: "2",
    studentName: "Bob Smith",
    rollNumber: "2024002",
    class: "10A", 
    status: "late",
    time: "09:15 AM",
    date: "2024-12-27",
  },
  {
    id: "3",
    studentName: "Carol Davis",
    rollNumber: "2024003",
    class: "10A",
    status: "absent",
    time: "-",
    date: "2024-12-27",
  },
  {
    id: "4",
    studentName: "David Wilson",
    rollNumber: "2024004",
    class: "10A",
    status: "present",
    time: "08:25 AM",
    date: "2024-12-27",
  },
];

export default function Attendance() {
  const { user } = useAuth();
  const [selectedDate, setSelectedDate] = useState<Date | undefined>(new Date());
  const [selectedClass, setSelectedClass] = useState("10A");
  const [attendanceData, setAttendanceData] = useState(attendanceRecords);
  const [sortColumn, setSortColumn] = useState<string>("");
  const [sortDirection, setSortDirection] = useState<"asc" | "desc">("asc");

  if (!user) return null;

  const updateAttendance = (studentId: string, status: 'present' | 'absent' | 'late') => {
    setAttendanceData(prev => 
      prev.map(record => 
        record.id === studentId 
          ? { ...record, status, time: status === 'absent' ? '-' : '08:30 AM' }
          : record
      )
    );
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case 'present':
        return <Badge className="bg-status-present text-white">Present</Badge>;
      case 'absent':
        return <Badge className="bg-status-absent text-white">Absent</Badge>;
      case 'late':
        return <Badge className="bg-status-late text-white">Late</Badge>;
      default:
        return <Badge className="bg-status-pending text-white">Pending</Badge>;
    }
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'present':
        return <Check className="h-4 w-4 text-status-present" />;
      case 'absent':
        return <X className="h-4 w-4 text-status-absent" />;
      case 'late':
        return <Clock className="h-4 w-4 text-status-late" />;
      default:
        return <Clock className="h-4 w-4 text-status-pending" />;
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

  const sortedAttendanceData = [...attendanceData].sort((a, b) => {
    if (!sortColumn) return 0;
    
    let aValue: any = a[sortColumn as keyof typeof a];
    let bValue: any = b[sortColumn as keyof typeof b];
    
    if (typeof aValue === "string") aValue = aValue.toLowerCase();
    if (typeof bValue === "string") bValue = bValue.toLowerCase();
    
    if (aValue < bValue) return sortDirection === "asc" ? -1 : 1;
    if (aValue > bValue) return sortDirection === "asc" ? 1 : -1;
    return 0;
  });

  const stats = {
    present: attendanceData.filter(r => r.status === 'present').length,
    absent: attendanceData.filter(r => r.status === 'absent').length,
    late: attendanceData.filter(r => r.status === 'late').length,
    total: attendanceData.length,
  };

  const attendancePercentage = ((stats.present + stats.late) / stats.total * 100).toFixed(1);

  if (user.role === 'student') {
    // Student view - show their own attendance
    return (
      <div className="space-y-6">
        <div>
          <h1 className="text-3xl font-bold text-foreground">My Attendance</h1>
          <p className="text-muted-foreground">View your attendance records and statistics</p>
        </div>

        <div className="grid gap-4 md:grid-cols-4">
          <Card>
            <CardHeader className="pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">
                Overall Attendance
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold text-education-green">94.2%</div>
              <p className="text-xs text-muted-foreground">This semester</p>
            </CardContent>
          </Card>
          <Card>
            <CardHeader className="pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">
                Present Days
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">142</div>
              <p className="text-xs text-muted-foreground">Out of 151 days</p>
            </CardContent>
          </Card>
          <Card>
            <CardHeader className="pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">
                Absent Days
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold text-status-absent">9</div>
              <p className="text-xs text-muted-foreground">This semester</p>
            </CardContent>
          </Card>
          <Card>
            <CardHeader className="pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">
                Late Arrivals
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold text-status-late">3</div>
              <p className="text-xs text-muted-foreground">This month</p>
            </CardContent>
          </Card>
        </div>

        <Card>
          <CardHeader>
            <CardTitle>Recent Attendance</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {attendanceRecords.slice(0, 10).map((record, index) => (
                <div key={index} className="flex items-center justify-between p-3 bg-muted/30 rounded-lg">
                  <div className="flex items-center space-x-3">
                    {getStatusIcon(record.status)}
                    <div>
                      <p className="font-medium">{format(new Date(record.date), 'MMMM d, yyyy')}</p>
                      <p className="text-sm text-muted-foreground">Arrival: {record.time}</p>
                    </div>
                  </div>
                  {getStatusBadge(record.status)}
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>
    );
  }

  // Teacher/Admin view - mark attendance
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-foreground">Attendance Management</h1>
          <p className="text-muted-foreground">Mark and manage student attendance</p>
        </div>
        <Button className="bg-gradient-primary">
          <UserCheck className="mr-2 h-4 w-4" />
          Mark All Present
        </Button>
      </div>

      {/* Stats Cards */}
      <div className="grid gap-4 md:grid-cols-5">
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Total Students
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{stats.total}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Present
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-status-present">{stats.present}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Absent
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-status-absent">{stats.absent}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Late
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-status-late">{stats.late}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Attendance Rate
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-education-green">{attendancePercentage}%</div>
          </CardContent>
        </Card>
      </div>

      {/* Controls */}
      <Card>
        <CardContent className="pt-6">
          <div className="flex gap-4 items-center mb-6">
            <div className="flex items-center space-x-2">
              <CalendarIcon className="h-4 w-4" />
              <span className="text-sm font-medium">
                {selectedDate ? format(selectedDate, 'MMMM d, yyyy') : 'Select date'}
              </span>
            </div>
            <Select value={selectedClass} onValueChange={setSelectedClass}>
              <SelectTrigger className="w-48">
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="10A">Class 10A</SelectItem>
                <SelectItem value="10B">Class 10B</SelectItem>
                <SelectItem value="9A">Class 9A</SelectItem>
                <SelectItem value="11A">Class 11A</SelectItem>
                <SelectItem value="12A">Class 12A</SelectItem>
              </SelectContent>
            </Select>
          </div>

          {/* Attendance Table */}
          <div className="rounded-md border">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("studentName")}
                  >
                    <div className="flex items-center">
                      Student
                      {getSortIcon("studentName")}
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
                    onClick={() => handleSort("status")}
                  >
                    <div className="flex items-center">
                      Status
                      {getSortIcon("status")}
                    </div>
                  </TableHead>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("time")}
                  >
                    <div className="flex items-center">
                      Time
                      {getSortIcon("time")}
                    </div>
                  </TableHead>
                  <TableHead>Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {sortedAttendanceData.map((record) => (
                  <TableRow key={record.id}>
                    <TableCell>
                      <div className="flex items-center space-x-3">
                        <Avatar>
                          <AvatarFallback>
                            {record.studentName.split(' ').map(n => n[0]).join('')}
                          </AvatarFallback>
                        </Avatar>
                        <div className="font-medium">{record.studentName}</div>
                      </div>
                    </TableCell>
                    <TableCell className="font-mono">{record.rollNumber}</TableCell>
                    <TableCell>{getStatusBadge(record.status)}</TableCell>
                    <TableCell>{record.time}</TableCell>
                    <TableCell>
                      <div className="flex items-center space-x-1">
                        <Button
                          variant={record.status === 'present' ? 'default' : 'outline'}
                          size="sm"
                          onClick={() => updateAttendance(record.id, 'present')}
                          className={record.status === 'present' ? 'bg-status-present' : ''}
                        >
                          <Check className="h-3 w-3" />
                        </Button>
                        <Button
                          variant={record.status === 'late' ? 'default' : 'outline'}
                          size="sm"
                          onClick={() => updateAttendance(record.id, 'late')}
                          className={record.status === 'late' ? 'bg-status-late' : ''}
                        >
                          <Clock className="h-3 w-3" />
                        </Button>
                        <Button
                          variant={record.status === 'absent' ? 'default' : 'outline'}
                          size="sm"
                          onClick={() => updateAttendance(record.id, 'absent')}
                          className={record.status === 'absent' ? 'bg-status-absent' : ''}
                        >
                          <X className="h-3 w-3" />
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