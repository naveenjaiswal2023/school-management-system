import { useAuth } from "@/contexts/AuthContext";
import { StatsCard } from "@/components/dashboard/StatsCard";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import {
  Users,
  GraduationCap,
  BookOpen,
  DollarSign,
  UserCheck,
  TrendingUp,
  Calendar,
  Bell,
} from "lucide-react";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  LineChart,
  Line,
  PieChart,
  Pie,
  Cell,
} from "recharts";

// Sample data for charts
const attendanceData = [
  { name: "Mon", present: 85, absent: 15 },
  { name: "Tue", present: 90, absent: 10 },
  { name: "Wed", present: 78, absent: 22 },
  { name: "Thu", present: 88, absent: 12 },
  { name: "Fri", present: 82, absent: 18 },
];

const performanceData = [
  { name: "Math", score: 85 },
  { name: "Science", score: 92 },
  { name: "English", score: 78 },
  { name: "History", score: 88 },
  { name: "Art", score: 94 },
];

const feeStatusData = [
  { name: "Paid", value: 75, color: "hsl(var(--education-green))" },
  { name: "Pending", value: 20, color: "hsl(var(--education-orange))" },
  { name: "Overdue", value: 5, color: "hsl(var(--destructive))" },
];

function AdminDashboard() {
  return (
    <div className="space-y-6">
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <StatsCard
          title="Total Students"
          value="1,248"
          description="Active enrollments"
          icon={GraduationCap}
          trend={{ value: 12, isPositive: true }}
        />
        <StatsCard
          title="Total Staff"
          value="87"
          description="Teaching & Non-teaching"
          icon={Users}
          trend={{ value: 5, isPositive: true }}
        />
        <StatsCard
          title="Attendance Rate"
          value="88.5%"
          description="This month average"
          icon={UserCheck}
          trend={{ value: 3, isPositive: true }}
        />
        <StatsCard
          title="Revenue"
          value="$125,000"
          description="Monthly collection"
          icon={DollarSign}
          trend={{ value: 8, isPositive: true }}
        />
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        <Card className="card-gradient shadow-elevated glow-on-hover group relative overflow-hidden">
          {/* Gradient overlay */}
          <div className="absolute inset-0 bg-gradient-to-br from-education-blue/5 via-transparent to-education-green/5 opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
          
          <CardHeader className="relative z-10">
            <CardTitle className="flex items-center gap-2 text-lg">
              <div className="p-2 rounded-lg bg-education-blue/10">
                📊
              </div>
              Weekly Attendance
            </CardTitle>
          </CardHeader>
          <CardContent className="relative z-10">
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={attendanceData}>
                <CartesianGrid strokeDasharray="3 3" opacity={0.3} />
                <XAxis dataKey="name" />
                <YAxis />
                <Tooltip />
                <Bar dataKey="present" fill="hsl(var(--education-green))" radius={[4, 4, 0, 0]} />
                <Bar dataKey="absent" fill="hsl(var(--destructive))" radius={[4, 4, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        <Card className="card-gradient shadow-elevated glow-on-hover group relative overflow-hidden">
          {/* Gradient overlay */}
          <div className="absolute inset-0 bg-gradient-to-br from-brand-primary/5 via-transparent to-brand-accent/5 opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
          
          <CardHeader className="relative z-10">
            <CardTitle className="flex items-center gap-2 text-lg">
              <div className="p-2 rounded-lg bg-brand-primary/10">
                💰
              </div>
              Fee Collection Status
            </CardTitle>
          </CardHeader>
          <CardContent className="relative z-10">
            <ResponsiveContainer width="100%" height={300}>
              <PieChart>
                <Pie
                  data={feeStatusData}
                  cx="50%"
                  cy="50%"
                  innerRadius={60}
                  outerRadius={120}
                  paddingAngle={5}
                  dataKey="value"
                >
                  {feeStatusData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.color} />
                  ))}
                </Pie>
                <Tooltip />
              </PieChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}

function TeacherDashboard() {
  return (
    <div className="space-y-6">
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <StatsCard
          title="My Classes"
          value="8"
          description="Active classes"
          icon={BookOpen}
          trend={{ value: 2, isPositive: true }}
        />
        <StatsCard
          title="Students"
          value="240"
          description="Under my guidance"
          icon={Users}
        />
        <StatsCard
          title="Attendance Today"
          value="92%"
          description="Class average"
          icon={UserCheck}
          trend={{ value: 5, isPositive: true }}
        />
        <StatsCard
          title="Pending Reviews"
          value="15"
          description="Assignments to grade"
          icon={Calendar}
        />
      </div>

      <div className="grid gap-4 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Class Performance</CardTitle>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={performanceData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" />
                <YAxis />
                <Tooltip />
                <Line 
                  type="monotone" 
                  dataKey="score" 
                  stroke="hsl(var(--brand-primary))" 
                  strokeWidth={3}
                />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Recent Activities</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <div className="flex items-center space-x-3">
                <div className="w-2 h-2 bg-education-green rounded-full"></div>
                <div className="flex-1">
                  <p className="text-sm font-medium">Math Quiz - Grade 10A</p>
                  <p className="text-xs text-muted-foreground">Completed 2 hours ago</p>
                </div>
              </div>
              <div className="flex items-center space-x-3">
                <div className="w-2 h-2 bg-education-orange rounded-full"></div>
                <div className="flex-1">
                  <p className="text-sm font-medium">Assignment submission</p>
                  <p className="text-xs text-muted-foreground">Due tomorrow</p>
                </div>
              </div>
              <div className="flex items-center space-x-3">
                <div className="w-2 h-2 bg-brand-primary rounded-full"></div>
                <div className="flex-1">
                  <p className="text-sm font-medium">Parent-Teacher Meeting</p>
                  <p className="text-xs text-muted-foreground">Scheduled for Friday</p>
                </div>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}

function StudentDashboard() {
  return (
    <div className="space-y-6">
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <StatsCard
          title="Attendance"
          value="94%"
          description="This semester"
          icon={UserCheck}
          trend={{ value: 2, isPositive: true }}
        />
        <StatsCard
          title="GPA"
          value="3.8"
          description="Current semester"
          icon={TrendingUp}
          trend={{ value: 5, isPositive: true }}
        />
        <StatsCard
          title="Assignments"
          value="3"
          description="Pending submissions"
          icon={BookOpen}
        />
        <StatsCard
          title="Fee Status"
          value="Paid"
          description="Current semester"
          icon={DollarSign}
          className="text-education-green"
        />
      </div>

      <div className="grid gap-4 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>My Performance</CardTitle>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={performanceData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" />
                <YAxis />
                <Tooltip />
                <Bar dataKey="score" fill="hsl(var(--brand-primary))" />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Upcoming Events</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <div className="flex items-center space-x-3 p-3 bg-muted/50 rounded-lg">
                <Calendar className="h-4 w-4 text-brand-primary" />
                <div className="flex-1">
                  <p className="text-sm font-medium">Final Exams</p>
                  <p className="text-xs text-muted-foreground">Dec 15-20, 2024</p>
                </div>
              </div>
              <div className="flex items-center space-x-3 p-3 bg-muted/50 rounded-lg">
                <Bell className="h-4 w-4 text-education-orange" />
                <div className="flex-1">
                  <p className="text-sm font-medium">Assignment Due</p>
                  <p className="text-xs text-muted-foreground">Chemistry Lab Report</p>
                </div>
              </div>
              <div className="flex items-center space-x-3 p-3 bg-muted/50 rounded-lg">
                <Users className="h-4 w-4 text-education-green" />
                <div className="flex-1">
                  <p className="text-sm font-medium">Sports Day</p>
                  <p className="text-xs text-muted-foreground">Dec 10, 2024</p>
                </div>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}

export default function Dashboard() {
  const { user } = useAuth();

  if (!user) return null;

  const renderDashboard = () => {
  if (user.roles.includes("admin")) {
    return <AdminDashboard />;
  }
  if (user.roles.includes("teacher")) {
    return <TeacherDashboard />;
  }
  if (user.roles.includes("student")) {
    return <StudentDashboard />;
  }
  if (user.roles.includes("hr")) {
    return <AdminDashboard />; // HR can use admin dashboard for now
  }
  if (user.roles.includes("accountant")) {
    return <AdminDashboard />; // Accountant can use admin dashboard for now
  }

  // Default fallback
  return <AdminDashboard />;
};

  return (
    <div className="space-y-8">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-4xl font-bold bg-gradient-primary bg-clip-text text-transparent mb-2">
            Dashboard
          </h1>
          <p className="text-lg text-muted-foreground">
            Welcome to your {user.roles} dashboard, {user.firstName.split(' ')[0]} ✨
          </p>
        </div>
        <div className="flex gap-3">
          <Button variant="premium" size="lg" className="hidden md:flex">
            Generate Report
          </Button>
          <Button variant="gradient" size="lg">
            Quick Actions
          </Button>
        </div>
      </div>
      
      {renderDashboard()}
    </div>
  );
}