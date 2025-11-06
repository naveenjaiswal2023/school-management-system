/**
 * Payroll Management Module
 * Generate and manage employee payroll with payslip generation
 * Features: Payroll generation, History, Payslip download, Search & Filter
 */

import { useState, useEffect } from "react";
import { Plus, Search, Download, Eye, DollarSign, Calendar, TrendingUp } from "lucide-react";
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
import { payrollAPI, employeeAPI, PayrollRecord, Employee } from "@/services/mockData";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

export default function Payroll() {
  const [payrollRecords, setPayrollRecords] = useState<PayrollRecord[]>([]);
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [filteredRecords, setFilteredRecords] = useState<PayrollRecord[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [statusFilter, setStatusFilter] = useState<string>("all");
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [isViewDialogOpen, setIsViewDialogOpen] = useState(false);
  const [selectedRecord, setSelectedRecord] = useState<PayrollRecord | null>(null);
  const [formData, setFormData] = useState<Partial<PayrollRecord>>({
    employeeId: "",
    employeeName: "",
    month: new Date().toLocaleString('default', { month: 'long' }),
    year: new Date().getFullYear(),
    basicSalary: 0,
    allowances: 0,
    deductions: 0,
    netSalary: 0,
    status: "pending",
    generatedDate: new Date().toISOString().split('T')[0],
  });

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    let filtered = payrollRecords.filter(
      (record) =>
        record.employeeName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        record.month.toLowerCase().includes(searchTerm.toLowerCase())
    );

    if (statusFilter !== "all") {
      filtered = filtered.filter((record) => record.status === statusFilter);
    }

    setFilteredRecords(filtered);
  }, [searchTerm, statusFilter, payrollRecords]);

  useEffect(() => {
    // Auto-calculate net salary
    const basic = formData.basicSalary || 0;
    const allowances = formData.allowances || 0;
    const deductions = formData.deductions || 0;
    const net = basic + allowances - deductions;
    setFormData(prev => ({ ...prev, netSalary: net }));
  }, [formData.basicSalary, formData.allowances, formData.deductions]);

  const loadData = async () => {
    try {
      const [payrollData, employeeData] = await Promise.all([
        payrollAPI.getAll(),
        employeeAPI.getAll()
      ]);
      setPayrollRecords(payrollData);
      setFilteredRecords(payrollData);
      setEmployees(employeeData);
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to load payroll data",
        variant: "destructive",
      });
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      await payrollAPI.create(formData as Omit<PayrollRecord, "id">);
      toast({
        title: "Success",
        description: "Payroll generated successfully",
      });
      loadData();
      resetForm();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to generate payroll",
        variant: "destructive",
      });
    }
  };

  const handleMarkAsPaid = async (record: PayrollRecord) => {
    try {
      await payrollAPI.update(record.id, {
        status: "paid",
        paidDate: new Date().toISOString().split('T')[0],
      });
      toast({
        title: "Success",
        description: "Payroll marked as paid",
      });
      loadData();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to update payroll status",
        variant: "destructive",
      });
    }
  };

  const handleView = (record: PayrollRecord) => {
    setSelectedRecord(record);
    setIsViewDialogOpen(true);
  };

  const handleDownloadPayslip = (record: PayrollRecord) => {
    // Simulate payslip download
    toast({
      title: "Download Started",
      description: `Payslip for ${record.employeeName} - ${record.month} ${record.year}`,
    });
  };

  const handleEmployeeChange = (employeeId: string) => {
    const employee = employees.find(e => e.id === employeeId);
    if (employee) {
      setFormData({
        ...formData,
        employeeId: employee.id,
        employeeName: employee.name,
        basicSalary: employee.salary,
      });
    }
  };

  const resetForm = () => {
    setFormData({
      employeeId: "",
      employeeName: "",
      month: new Date().toLocaleString('default', { month: 'long' }),
      year: new Date().getFullYear(),
      basicSalary: 0,
      allowances: 0,
      deductions: 0,
      netSalary: 0,
      status: "pending",
      generatedDate: new Date().toISOString().split('T')[0],
    });
    setIsDialogOpen(false);
  };

  const totalPayroll = payrollRecords.reduce((sum, r) => sum + r.netSalary, 0);
  const pendingPayroll = payrollRecords
    .filter(r => r.status === 'pending')
    .reduce((sum, r) => sum + r.netSalary, 0);
  const paidPayroll = payrollRecords
    .filter(r => r.status === 'paid')
    .reduce((sum, r) => sum + r.netSalary, 0);

  return (
    <div className="p-6 space-y-6">
      {/* Header Section */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold bg-gradient-primary bg-clip-text text-transparent">
            Payroll Management
          </h1>
          <p className="text-muted-foreground mt-1">
            Generate and manage employee payroll and payslips
          </p>
        </div>
        <Button onClick={() => setIsDialogOpen(true)} className="btn-professional">
          <Plus className="mr-2 h-4 w-4" />
          Generate Payroll
        </Button>
      </div>

      {/* Search and Filter */}
      <Card className="p-4 card-gradient glow-on-hover">
        <div className="flex gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground h-4 w-4" />
            <Input
              placeholder="Search by employee name or month..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="pl-10"
            />
          </div>
          <Select value={statusFilter} onValueChange={setStatusFilter}>
            <SelectTrigger className="w-[180px]">
              <SelectValue placeholder="Filter by status" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">All Status</SelectItem>
              <SelectItem value="pending">Pending</SelectItem>
              <SelectItem value="paid">Paid</SelectItem>
            </SelectContent>
          </Select>
        </div>
      </Card>

      {/* Statistics Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Total Payroll</p>
              <p className="text-2xl font-bold">${(totalPayroll / 1000).toFixed(0)}K</p>
            </div>
            <DollarSign className="h-8 w-8 text-primary" />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Pending</p>
              <p className="text-2xl font-bold">${(pendingPayroll / 1000).toFixed(0)}K</p>
            </div>
            <Calendar className="h-8 w-8" style={{ color: 'hsl(var(--education-orange))' }} />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Paid</p>
              <p className="text-2xl font-bold">${(paidPayroll / 1000).toFixed(0)}K</p>
            </div>
            <TrendingUp className="h-8 w-8" style={{ color: 'hsl(var(--education-green))' }} />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Records</p>
              <p className="text-2xl font-bold">{payrollRecords.length}</p>
            </div>
            <Calendar className="h-8 w-8" style={{ color: 'hsl(var(--education-blue))' }} />
          </div>
        </Card>
      </div>

      {/* Payroll Table */}
      <Card className="card-gradient">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Employee</TableHead>
              <TableHead>Period</TableHead>
              <TableHead>Basic Salary</TableHead>
              <TableHead>Allowances</TableHead>
              <TableHead>Deductions</TableHead>
              <TableHead>Net Salary</TableHead>
              <TableHead>Status</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {filteredRecords.map((record) => (
              <TableRow key={record.id}>
                <TableCell className="font-medium">{record.employeeName}</TableCell>
                <TableCell>
                  {record.month} {record.year}
                </TableCell>
                <TableCell>${record.basicSalary.toLocaleString()}</TableCell>
                <TableCell className="text-green-600">
                  +${record.allowances.toLocaleString()}
                </TableCell>
                <TableCell className="text-red-600">
                  -${record.deductions.toLocaleString()}
                </TableCell>
                <TableCell className="font-bold">
                  ${record.netSalary.toLocaleString()}
                </TableCell>
                <TableCell>
                  <Badge
                    variant={record.status === "paid" ? "default" : "secondary"}
                  >
                    {record.status}
                  </Badge>
                </TableCell>
                <TableCell className="text-right">
                  <div className="flex justify-end gap-2">
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => handleView(record)}
                    >
                      <Eye className="h-4 w-4" />
                    </Button>
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => handleDownloadPayslip(record)}
                    >
                      <Download className="h-4 w-4" />
                    </Button>
                    {record.status === "pending" && (
                      <Button
                        variant="ghost"
                        size="sm"
                        onClick={() => handleMarkAsPaid(record)}
                      >
                        Mark Paid
                      </Button>
                    )}
                  </div>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Card>

      {/* Generate Payroll Dialog */}
      <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
        <DialogContent className="max-w-2xl">
          <DialogHeader>
            <DialogTitle>Generate Payroll</DialogTitle>
            <DialogDescription>
              Create a new payroll record for an employee
            </DialogDescription>
          </DialogHeader>
          <form onSubmit={handleSubmit}>
            <div className="grid gap-4 py-4">
              <div>
                <Label htmlFor="employee">Employee</Label>
                <Select
                  value={formData.employeeId}
                  onValueChange={handleEmployeeChange}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Select employee" />
                  </SelectTrigger>
                  <SelectContent>
                    {employees.map((emp) => (
                      <SelectItem key={emp.id} value={emp.id}>
                        {emp.name} - {emp.position}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label htmlFor="month">Month</Label>
                  <Select
                    value={formData.month}
                    onValueChange={(value) =>
                      setFormData({ ...formData, month: value })
                    }
                  >
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      {[
                        "January", "February", "March", "April", "May", "June",
                        "July", "August", "September", "October", "November", "December"
                      ].map((month) => (
                        <SelectItem key={month} value={month}>
                          {month}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
                <div>
                  <Label htmlFor="year">Year</Label>
                  <Input
                    id="year"
                    type="number"
                    value={formData.year}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        year: parseInt(e.target.value),
                      })
                    }
                    required
                  />
                </div>
              </div>

              <div>
                <Label htmlFor="basicSalary">Basic Salary ($)</Label>
                <Input
                  id="basicSalary"
                  type="number"
                  value={formData.basicSalary}
                  onChange={(e) =>
                    setFormData({
                      ...formData,
                      basicSalary: parseFloat(e.target.value) || 0,
                    })
                  }
                  required
                />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label htmlFor="allowances">Allowances ($)</Label>
                  <Input
                    id="allowances"
                    type="number"
                    value={formData.allowances}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        allowances: parseFloat(e.target.value) || 0,
                      })
                    }
                  />
                </div>
                <div>
                  <Label htmlFor="deductions">Deductions ($)</Label>
                  <Input
                    id="deductions"
                    type="number"
                    value={formData.deductions}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        deductions: parseFloat(e.target.value) || 0,
                      })
                    }
                  />
                </div>
              </div>

              <div className="p-4 bg-muted rounded-lg">
                <div className="flex justify-between items-center">
                  <span className="text-lg font-semibold">Net Salary:</span>
                  <span className="text-2xl font-bold text-primary">
                    ${formData.netSalary?.toLocaleString()}
                  </span>
                </div>
              </div>
            </div>
            <DialogFooter>
              <Button type="button" variant="outline" onClick={resetForm}>
                Cancel
              </Button>
              <Button type="submit" className="btn-professional">
                Generate Payroll
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>

      {/* View Payslip Dialog */}
      <Dialog open={isViewDialogOpen} onOpenChange={setIsViewDialogOpen}>
        <DialogContent className="max-w-2xl">
          <DialogHeader>
            <DialogTitle>Payslip Details</DialogTitle>
          </DialogHeader>
          {selectedRecord && (
            <div className="space-y-6">
              {/* Header */}
              <div className="text-center border-b pb-4">
                <h2 className="text-2xl font-bold">EduManage School</h2>
                <p className="text-sm text-muted-foreground">Payslip for {selectedRecord.month} {selectedRecord.year}</p>
              </div>

              {/* Employee Details */}
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label className="text-muted-foreground">Employee Name</Label>
                  <p className="font-medium">{selectedRecord.employeeName}</p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Employee ID</Label>
                  <p className="font-medium">{selectedRecord.employeeId}</p>
                </div>
              </div>

              {/* Salary Breakdown */}
              <div className="space-y-3">
                <h3 className="font-semibold">Salary Breakdown</h3>
                <div className="space-y-2">
                  <div className="flex justify-between p-2 bg-muted rounded">
                    <span>Basic Salary</span>
                    <span className="font-medium">${selectedRecord.basicSalary.toLocaleString()}</span>
                  </div>
                  <div className="flex justify-between p-2 bg-muted rounded">
                    <span className="text-green-600">Allowances</span>
                    <span className="font-medium text-green-600">
                      +${selectedRecord.allowances.toLocaleString()}
                    </span>
                  </div>
                  <div className="flex justify-between p-2 bg-muted rounded">
                    <span className="text-red-600">Deductions</span>
                    <span className="font-medium text-red-600">
                      -${selectedRecord.deductions.toLocaleString()}
                    </span>
                  </div>
                  <div className="flex justify-between p-3 bg-primary/10 rounded-lg border-2 border-primary">
                    <span className="text-lg font-semibold">Net Salary</span>
                    <span className="text-xl font-bold text-primary">
                      ${selectedRecord.netSalary.toLocaleString()}
                    </span>
                  </div>
                </div>
              </div>

              {/* Payment Details */}
              <div className="grid grid-cols-2 gap-4 pt-4 border-t">
                <div>
                  <Label className="text-muted-foreground">Generated Date</Label>
                  <p className="font-medium">
                    {new Date(selectedRecord.generatedDate).toLocaleDateString()}
                  </p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Payment Status</Label>
                  <div className="mt-1">
                    <Badge variant={selectedRecord.status === "paid" ? "default" : "secondary"}>
                      {selectedRecord.status}
                    </Badge>
                  </div>
                </div>
                {selectedRecord.paidDate && (
                  <div>
                    <Label className="text-muted-foreground">Paid Date</Label>
                    <p className="font-medium">
                      {new Date(selectedRecord.paidDate).toLocaleDateString()}
                    </p>
                  </div>
                )}
              </div>

              {/* Actions */}
              <div className="flex justify-end gap-2 pt-4 border-t">
                <Button
                  variant="outline"
                  onClick={() => handleDownloadPayslip(selectedRecord)}
                >
                  <Download className="mr-2 h-4 w-4" />
                  Download PDF
                </Button>
                {selectedRecord.status === "pending" && (
                  <Button
                    onClick={() => {
                      handleMarkAsPaid(selectedRecord);
                      setIsViewDialogOpen(false);
                    }}
                    className="btn-professional"
                  >
                    Mark as Paid
                  </Button>
                )}
              </div>
            </div>
          )}
        </DialogContent>
      </Dialog>
    </div>
  );
}
