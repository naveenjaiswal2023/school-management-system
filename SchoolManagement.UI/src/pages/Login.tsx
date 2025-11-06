import { useState } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "@/contexts/AuthContext";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { GraduationCap, Eye, EyeOff } from "lucide-react";
import { useToast } from "@/hooks/use-toast";

export default function Login() {
  const { user, login, isLoading } = useAuth();
  const { toast } = useToast();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  if (user) {
    return <Navigate to="/dashboard" replace />;
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      await login(email, password);
      toast({
        title: "Welcome back!",
        description: "You have successfully logged in.",
      });
    } catch (error) {
      toast({
        title: "Login failed",
        description: "Invalid email or password. Please try again.",
        variant: "destructive",
      });
    }
  };

  // const demoCredentials = [
  //   { email: "admin@school.edu", role: "Admin", name: "Dr. Sarah Johnson" },
  //   { email: "teacher@school.edu", role: "Teacher", name: "Prof. Michael Chen" },
  //   { email: "student@school.edu", role: "Student", name: "Emma Rodriguez" },
  //   { email: "hr@school.edu", role: "HR", name: "Lisa Thompson" },
  //   { email: "accountant@school.edu", role: "Accountant", name: "David Park" },
  // ];

  return (
    <div className="min-h-screen bg-gradient-primary flex items-center justify-center p-4">
      <div className="w-full max-w-md">
        <Card className="shadow-brand">
          <CardHeader className="text-center">
            <div className="mx-auto w-12 h-12 bg-gradient-accent rounded-lg flex items-center justify-center mb-4">
              <GraduationCap className="h-6 w-6 text-white" />
            </div>
            <CardTitle className="text-2xl font-bold">EduManage</CardTitle>
            <CardDescription>
              Sign in to your School Management System
            </CardDescription>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="email">Email</Label>
                <Input
                  id="email"
                  type="email"
                  placeholder="Enter your email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="password">Password</Label>
                <div className="relative">
                  <Input
                    id="password"
                    type={showPassword ? "text" : "password"}
                    placeholder="Enter your password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                  />
                  <Button
                    type="button"
                    variant="ghost"
                    size="sm"
                    className="absolute right-0 top-0 h-full px-3 py-2"
                    onClick={() => setShowPassword(!showPassword)}
                  >
                    {showPassword ? (
                      <EyeOff className="h-4 w-4" />
                    ) : (
                      <Eye className="h-4 w-4" />
                    )}
                  </Button>
                </div>
              </div>
              <Button
                type="submit"
                className="w-full bg-gradient-primary hover:opacity-90"
                disabled={isLoading}
              >
                {isLoading ? "Signing in..." : "Sign In"}
              </Button>
            </form>

            {/* <div className="mt-6">
              <p className="text-sm text-center text-muted-foreground mb-3">
                Demo Credentials (password: "password")
              </p>
              <div className="grid gap-2">
                {demoCredentials.map((cred) => (
                  <Button
                    key={cred.email}
                    variant="outline"
                    size="sm"
                    className="justify-between text-xs"
                    onClick={() => {
                      setEmail(cred.email);
                      setPassword("password");
                    }}
                  >
                    <span>{cred.name}</span>
                    <span className="text-muted-foreground">{cred.role}</span>
                  </Button>
                ))}
              </div>
            </div> */}
          </CardContent>
        </Card>
      </div>
    </div>
  );
}