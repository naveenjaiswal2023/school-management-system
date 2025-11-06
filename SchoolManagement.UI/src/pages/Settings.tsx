import { useState, useMemo } from "react";
import { useAuth } from "@/contexts/AuthContext";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Switch } from "@/components/ui/switch";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Separator } from "@/components/ui/separator";
import { toast } from "@/hooks/use-toast";
import { Settings as SettingsIcon, School, Bell, Shield, Database, Mail } from "lucide-react";

// 🧩 Define a safe type for your user roles
type UserRole = string | string[] | undefined;

export default function Settings() {
  const { user } = useAuth() as { user?: { roles?: UserRole } };

  const [emailNotifications, setEmailNotifications] = useState(true);
  const [pushNotifications, setPushNotifications] = useState(true);
  const [academicYear, setAcademicYear] = useState("2024-2025");
  const [schoolName, setSchoolName] = useState("Modern School Management System");
  const [timeZone, setTimeZone] = useState("UTC");

  // Safe role-based access check
  const hasAdminAccess = useMemo(() => {
    if (!user?.roles) return false;

    const rolesArray = Array.isArray(user.roles)
      ? user.roles
      : [user.roles];

    return rolesArray.some(
      (r) => typeof r === "string" && r.toLowerCase() === "admin"
    );
  }, [user]);

  if (!hasAdminAccess) {
    return (
      <div className="flex items-center justify-center h-64">
        <p className="text-muted-foreground">
          Access denied. This page is only available to administrators.
        </p>
      </div>
    );
  }

  const handleSaveGeneral = () => {
    toast({
      title: "Settings saved",
      description: "General settings have been updated successfully.",
    });
  };

  const handleSaveNotifications = () => {
    toast({
      title: "Preferences saved",
      description: "Notification preferences have been updated successfully.",
    });
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center gap-3">
        <SettingsIcon className="h-8 w-8 text-primary" />
        <div>
          <h1 className="text-3xl font-bold text-foreground">System Settings</h1>
          <p className="text-muted-foreground">Configure your school management system</p>
        </div>
      </div>

      <Tabs defaultValue="general" className="space-y-4">
        <TabsList className="grid w-full grid-cols-5">
          <TabsTrigger value="general">General</TabsTrigger>
          <TabsTrigger value="academic">Academic</TabsTrigger>
          <TabsTrigger value="notifications">Notifications</TabsTrigger>
          <TabsTrigger value="security">Security</TabsTrigger>
          <TabsTrigger value="integrations">Integrations</TabsTrigger>
        </TabsList>

        <TabsContent value="general" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <School className="h-5 w-5" />
                School Information
              </CardTitle>
              <CardDescription>Update your school's basic information</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="schoolName">School Name</Label>
                <Input
                  id="schoolName"
                  value={schoolName}
                  onChange={(e) => setSchoolName(e.target.value)}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="email">Contact Email</Label>
                <Input
                  id="email"
                  type="email"
                  placeholder="contact@school.edu"
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="phone">Contact Phone</Label>
                <Input
                  id="phone"
                  type="tel"
                  placeholder="+1 (555) 000-0000"
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="address">Address</Label>
                <Input
                  id="address"
                  placeholder="123 Education St, City, State"
                />
              </div>
              <Separator />
              <div className="space-y-2">
                <Label htmlFor="timezone">Time Zone</Label>
                <Select value={timeZone} onValueChange={setTimeZone}>
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="UTC">UTC (Coordinated Universal Time)</SelectItem>
                    <SelectItem value="EST">EST (Eastern Standard Time)</SelectItem>
                    <SelectItem value="PST">PST (Pacific Standard Time)</SelectItem>
                    <SelectItem value="IST">IST (Indian Standard Time)</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <Button onClick={handleSaveGeneral} className="bg-gradient-primary">
                Save Changes
              </Button>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="academic" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Academic Configuration</CardTitle>
              <CardDescription>Configure academic year and term settings</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="academicYear">Academic Year</Label>
                <Select value={academicYear} onValueChange={setAcademicYear}>
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="2023-2024">2023-2024</SelectItem>
                    <SelectItem value="2024-2025">2024-2025</SelectItem>
                    <SelectItem value="2025-2026">2025-2026</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div className="space-y-2">
                <Label htmlFor="currentTerm">Current Term</Label>
                <Select defaultValue="1">
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="1">Term 1</SelectItem>
                    <SelectItem value="2">Term 2</SelectItem>
                    <SelectItem value="3">Term 3</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div className="space-y-2">
                <Label htmlFor="attendanceThreshold">Attendance Warning Threshold (%)</Label>
                <Input
                  id="attendanceThreshold"
                  type="number"
                  defaultValue="80"
                  min="0"
                  max="100"
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="passingGrade">Passing Grade (%)</Label>
                <Input
                  id="passingGrade"
                  type="number"
                  defaultValue="50"
                  min="0"
                  max="100"
                />
              </div>
              <Button onClick={handleSaveGeneral} className="bg-gradient-primary">
                Save Changes
              </Button>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="notifications" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Bell className="h-5 w-5" />
                Notification Preferences
              </CardTitle>
              <CardDescription>Configure how you want to receive notifications</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex items-center justify-between">
                <div className="space-y-0.5">
                  <Label>Email Notifications</Label>
                  <p className="text-sm text-muted-foreground">Receive notifications via email</p>
                </div>
                <Switch
                  checked={emailNotifications}
                  onCheckedChange={setEmailNotifications}
                />
              </div>
              <Separator />
              <div className="flex items-center justify-between">
                <div className="space-y-0.5">
                  <Label>Push Notifications</Label>
                  <p className="text-sm text-muted-foreground">Receive push notifications in browser</p>
                </div>
                <Switch
                  checked={pushNotifications}
                  onCheckedChange={setPushNotifications}
                />
              </div>
              <Separator />
              <div className="space-y-2">
                <Label>Notification Categories</Label>
                <div className="space-y-2">
                  <div className="flex items-center justify-between">
                    <Label className="font-normal">Student Activities</Label>
                    <Switch defaultChecked />
                  </div>
                  <div className="flex items-center justify-between">
                    <Label className="font-normal">Attendance Alerts</Label>
                    <Switch defaultChecked />
                  </div>
                  <div className="flex items-center justify-between">
                    <Label className="font-normal">Exam Results</Label>
                    <Switch defaultChecked />
                  </div>
                  <div className="flex items-center justify-between">
                    <Label className="font-normal">Fee Payments</Label>
                    <Switch defaultChecked />
                  </div>
                </div>
              </div>
              <Button onClick={handleSaveNotifications} className="bg-gradient-primary">
                Save Preferences
              </Button>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="security" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Shield className="h-5 w-5" />
                Security Settings
              </CardTitle>
              <CardDescription>Manage security and access control</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex items-center justify-between">
                <div className="space-y-0.5">
                  <Label>Two-Factor Authentication</Label>
                  <p className="text-sm text-muted-foreground">Add an extra layer of security</p>
                </div>
                <Switch />
              </div>
              <Separator />
              <div className="flex items-center justify-between">
                <div className="space-y-0.5">
                  <Label>Session Timeout</Label>
                  <p className="text-sm text-muted-foreground">Auto logout after inactivity</p>
                </div>
                <Switch defaultChecked />
              </div>
              <Separator />
              <div className="space-y-2">
                <Label htmlFor="sessionDuration">Session Duration (minutes)</Label>
                <Input
                  id="sessionDuration"
                  type="number"
                  defaultValue="30"
                  min="5"
                  max="480"
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="passwordPolicy">Password Policy</Label>
                <Select defaultValue="medium">
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="low">Low (8 characters)</SelectItem>
                    <SelectItem value="medium">Medium (8+ chars, numbers)</SelectItem>
                    <SelectItem value="high">High (12+ chars, special chars)</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <Button onClick={handleSaveGeneral} className="bg-gradient-primary">
                Save Changes
              </Button>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="integrations" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Database className="h-5 w-5" />
                External Integrations
              </CardTitle>
              <CardDescription>Connect with external services and APIs</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex items-center justify-between p-4 border rounded-lg">
                <div className="flex items-center gap-3">
                  <Mail className="h-5 w-5 text-muted-foreground" />
                  <div>
                    <p className="font-medium">Email Service</p>
                    <p className="text-sm text-muted-foreground">SMTP configuration</p>
                  </div>
                </div>
                <Button variant="outline">Configure</Button>
              </div>
              <div className="flex items-center justify-between p-4 border rounded-lg">
                <div className="flex items-center gap-3">
                  <Database className="h-5 w-5 text-muted-foreground" />
                  <div>
                    <p className="font-medium">SMS Gateway</p>
                    <p className="text-sm text-muted-foreground">Send SMS notifications</p>
                  </div>
                </div>
                <Button variant="outline">Configure</Button>
              </div>
              <div className="flex items-center justify-between p-4 border rounded-lg">
                <div className="flex items-center gap-3">
                  <Database className="h-5 w-5 text-muted-foreground" />
                  <div>
                    <p className="font-medium">Payment Gateway</p>
                    <p className="text-sm text-muted-foreground">Process fee payments</p>
                  </div>
                </div>
                <Button variant="outline">Configure</Button>
              </div>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}