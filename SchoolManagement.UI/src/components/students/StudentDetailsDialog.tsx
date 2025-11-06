import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import { Separator } from "@/components/ui/separator";
import { Mail, Phone, User, GraduationCap, Hash, Activity } from "lucide-react";

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

interface StudentDetailsDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  student: Student | null;
}

export function StudentDetailsDialog({ open, onOpenChange, student }: StudentDetailsDialogProps) {
  if (!student) return null;

  const getStatusBadge = (status: string, attendance: number) => {
    if (status === "Warning" || attendance < 80) {
      return <Badge variant="destructive">Warning</Badge>;
    }
    return <Badge className="bg-education-green text-white">Active</Badge>;
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>Student Details</DialogTitle>
        </DialogHeader>
        <div className="space-y-6">
          <div className="flex items-center gap-4">
            <Avatar className="h-16 w-16">
              <AvatarFallback className="text-lg">
                {student.name.split(' ').map(n => n[0]).join('')}
              </AvatarFallback>
            </Avatar>
            <div className="flex-1">
              <h3 className="text-xl font-semibold">{student.name}</h3>
              <p className="text-sm text-muted-foreground">{student.email}</p>
              <div className="mt-2">
                {getStatusBadge(student.status, student.attendance)}
              </div>
            </div>
          </div>

          <Separator />

          <div className="grid grid-cols-2 gap-4">
            <div className="flex items-center gap-3">
              <Hash className="h-5 w-5 text-muted-foreground" />
              <div>
                <p className="text-sm text-muted-foreground">Roll Number</p>
                <p className="font-medium font-mono">{student.rollNumber}</p>
              </div>
            </div>
            <div className="flex items-center gap-3">
              <GraduationCap className="h-5 w-5 text-muted-foreground" />
              <div>
                <p className="text-sm text-muted-foreground">Class</p>
                <p className="font-medium">{student.class}</p>
              </div>
            </div>
            <div className="flex items-center gap-3">
              <Activity className="h-5 w-5 text-muted-foreground" />
              <div>
                <p className="text-sm text-muted-foreground">Attendance</p>
                <p className="font-medium">{student.attendance}%</p>
              </div>
            </div>
            <div className="flex items-center gap-3">
              <User className="h-5 w-5 text-muted-foreground" />
              <div>
                <p className="text-sm text-muted-foreground">Parent/Guardian</p>
                <p className="font-medium">{student.parent}</p>
              </div>
            </div>
          </div>

          <Separator />

          <div className="space-y-3">
            <h4 className="font-semibold">Contact Information</h4>
            <div className="space-y-2">
              <div className="flex items-center gap-3 text-sm">
                <Mail className="h-4 w-4 text-muted-foreground" />
                <span>{student.email}</span>
              </div>
              <div className="flex items-center gap-3 text-sm">
                <Phone className="h-4 w-4 text-muted-foreground" />
                <span>{student.phone}</span>
              </div>
            </div>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}