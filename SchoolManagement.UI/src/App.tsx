import { Toaster } from "@/components/ui/toaster";
import { Toaster as Sonner } from "@/components/ui/sonner";
import { TooltipProvider } from "@/components/ui/tooltip";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider, useAuth } from "./contexts/AuthContext";
import { Layout } from "./components/layout/Layout";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Students from "./pages/Students";
import Attendance from "./pages/Attendance";
import UserManagement from "./pages/UserManagement";
import RoleManagement from "./pages/RoleManagement";
import Examinations from "./pages/Examinations";
import FeeManagement from "./pages/FeeManagement";
import EmployeeManagement from "./pages/EmployeeManagement";
import ClassManagement from "./pages/ClassManagement";
import DepartmentManagement from "./pages/DepartmentManagement";
import Payroll from "./pages/Payroll";
import ExamResults from "./pages/ExamResults";
import NotFound from "./pages/NotFound";
import Settings from "./pages/Settings";
import Notifications from "./pages/Notifications";

const queryClient = new QueryClient();

function PrivateRoute({ children }: { children: React.ReactNode }) {
  const { user, isLoading } = useAuth();
  
  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
      </div>
    );
  }
  
  return user ? <Layout>{children}</Layout> : <Navigate to="/login" replace />;
}

function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/dashboard" element={
        <PrivateRoute>
          <Dashboard />
        </PrivateRoute>
      } />
      <Route path="/students" element={
        <PrivateRoute>
          <Students />
        </PrivateRoute>
      } />
      <Route path="/students/add" element={
        <PrivateRoute>
          <div className="p-8 text-center">
            <h2 className="text-2xl font-bold mb-4">Add New Student</h2>
            <p className="text-muted-foreground">Student registration form coming soon...</p>
          </div>
        </PrivateRoute>
      } />
      <Route path="/attendance" element={
        <PrivateRoute>
          <Attendance />
        </PrivateRoute>
      } />
      <Route path="/attendance/mark" element={
        <PrivateRoute>
          <Attendance />
        </PrivateRoute>
      } />
      <Route path="/examinations" element={
        <PrivateRoute>
          <Examinations />
        </PrivateRoute>
      } />
      <Route path="/examinations/results" element={
        <PrivateRoute>
          <ExamResults />
        </PrivateRoute>
      } />
      <Route path="/fees" element={
        <PrivateRoute>
          <FeeManagement />
        </PrivateRoute>
      } />
      <Route path="/hrms/employees" element={
        <PrivateRoute>
          <EmployeeManagement />
        </PrivateRoute>
      } />
      <Route path="/hrms/payroll" element={
        <PrivateRoute>
          <Payroll />
        </PrivateRoute>
      } />
      <Route path="/classes" element={
        <PrivateRoute>
          <ClassManagement />
        </PrivateRoute>
      } />
      <Route path="/departments" element={
        <PrivateRoute>
          <DepartmentManagement />
        </PrivateRoute>
      } />
      <Route path="/hrms/*" element={
        <PrivateRoute>
          <EmployeeManagement />
        </PrivateRoute>
      } />
      <Route path="/users" element={
        <PrivateRoute>
          <UserManagement />
        </PrivateRoute>
      } />
      <Route path="/roles" element={
        <PrivateRoute>
          <RoleManagement />
        </PrivateRoute>
      } />
      <Route path="/notifications" element={
        <PrivateRoute>
          <Notifications />
        </PrivateRoute>
      } />
      <Route path="/settings" element={
        <PrivateRoute>
          <Settings />
        </PrivateRoute>
      } />
      <Route path="/" element={<Navigate to="/dashboard" replace />} />
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
}

const App = () => (
  <QueryClientProvider client={queryClient}>
    <AuthProvider>
      <TooltipProvider>
        <Toaster />
        <Sonner />
        <BrowserRouter>
          <AppRoutes />
        </BrowserRouter>
      </TooltipProvider>
    </AuthProvider>
  </QueryClientProvider>
);

export default App;
