import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
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
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { useToast } from "@/hooks/use-toast";
import { DollarSign, Plus, Search, FileText, CreditCard, Calendar, ArrowUpDown, ArrowUp, ArrowDown } from "lucide-react";
import { feeAPI, FeeRecord } from "@/services/mockData";
import { format } from "date-fns";

export default function FeeManagement() {
  const [feeRecords, setFeeRecords] = useState<FeeRecord[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [isPaymentDialogOpen, setIsPaymentDialogOpen] = useState(false);
  const [selectedRecord, setSelectedRecord] = useState<FeeRecord | null>(null);
  const [sortColumn, setSortColumn] = useState<string>("");
  const [sortDirection, setSortDirection] = useState<"asc" | "desc">("asc");
  const [formData, setFormData] = useState({
    studentName: "",
    class: "",
    feeType: "",
    amount: 0,
    dueDate: ""
  });
  const { toast } = useToast();

  useEffect(() => {
    loadFeeRecords();
  }, []);

  const loadFeeRecords = async () => {
    try {
      const data = await feeAPI.getAll();
      setFeeRecords(data);
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to load fee records",
        variant: "destructive",
      });
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await feeAPI.create({
        ...formData,
        studentId: Date.now().toString(),
        status: 'pending' as const
      });
      toast({
        title: "Success",
        description: "Fee record created successfully",
      });
      loadFeeRecords();
      handleCloseDialog();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to create fee record",
        variant: "destructive",
      });
    }
  };

  const handlePayment = async (recordId: string) => {
    try {
      await feeAPI.update(recordId, {
        status: 'paid',
        paidDate: new Date().toISOString().split('T')[0]
      });
      toast({
        title: "Success",
        description: "Payment recorded successfully",
      });
      loadFeeRecords();
      setIsPaymentDialogOpen(false);
      setSelectedRecord(null);
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to record payment",
        variant: "destructive",
      });
    }
  };

  const handleCloseDialog = () => {
    setIsDialogOpen(false);
    setFormData({
      studentName: "",
      class: "",
      feeType: "",
      amount: 0,
      dueDate: ""
    });
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

  const filteredRecords = feeRecords
    .filter(record =>
      record.studentName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      record.class.toLowerCase().includes(searchTerm.toLowerCase()) ||
      record.feeType.toLowerCase().includes(searchTerm.toLowerCase())
    )
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

  const openPaymentDialog = (record: FeeRecord) => {
    setSelectedRecord(record);
    setIsPaymentDialogOpen(true);
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case 'paid':
        return <Badge className="bg-status-present text-white">Paid</Badge>;
      case 'pending':
        return <Badge className="bg-status-late text-white">Pending</Badge>;
      case 'overdue':
        return <Badge className="bg-status-absent text-white">Overdue</Badge>;
      default:
        return <Badge className="bg-gray-500 text-white">{status}</Badge>;
    }
  };

  const stats = {
    total: feeRecords.length,
    paid: feeRecords.filter(r => r.status === 'paid').length,
    pending: feeRecords.filter(r => r.status === 'pending').length,
    overdue: feeRecords.filter(r => r.status === 'overdue').length,
    totalAmount: feeRecords.reduce((sum, r) => sum + r.amount, 0),
    collectedAmount: feeRecords.filter(r => r.status === 'paid').reduce((sum, r) => sum + r.amount, 0),
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-foreground">Fee Management</h1>
          <p className="text-muted-foreground">Manage student fees and payments</p>
        </div>
        
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button className="bg-gradient-primary">
              <Plus className="mr-2 h-4 w-4" />
              Add Fee Record
            </Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-[425px]">
            <DialogHeader>
              <DialogTitle>Add New Fee Record</DialogTitle>
            </DialogHeader>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="studentName">Student Name</Label>
                <Input
                  id="studentName"
                  value={formData.studentName}
                  onChange={(e) => setFormData({ ...formData, studentName: e.target.value })}
                  placeholder="Enter student name"
                  required
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="class">Class</Label>
                <Select value={formData.class} onValueChange={(value) => setFormData({ ...formData, class: value })}>
                  <SelectTrigger>
                    <SelectValue placeholder="Select class" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="9A">Class 9A</SelectItem>
                    <SelectItem value="9B">Class 9B</SelectItem>
                    <SelectItem value="10A">Class 10A</SelectItem>
                    <SelectItem value="10B">Class 10B</SelectItem>
                    <SelectItem value="11A">Class 11A</SelectItem>
                    <SelectItem value="12A">Class 12A</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div className="space-y-2">
                <Label htmlFor="feeType">Fee Type</Label>
                <Select value={formData.feeType} onValueChange={(value) => setFormData({ ...formData, feeType: value })}>
                  <SelectTrigger>
                    <SelectValue placeholder="Select fee type" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="Tuition Fee">Tuition Fee</SelectItem>
                    <SelectItem value="Library Fee">Library Fee</SelectItem>
                    <SelectItem value="Laboratory Fee">Laboratory Fee</SelectItem>
                    <SelectItem value="Sports Fee">Sports Fee</SelectItem>
                    <SelectItem value="Transport Fee">Transport Fee</SelectItem>
                    <SelectItem value="Exam Fee">Exam Fee</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div className="space-y-2">
                <Label htmlFor="amount">Amount</Label>
                <Input
                  id="amount"
                  type="number"
                  value={formData.amount}
                  onChange={(e) => setFormData({ ...formData, amount: parseInt(e.target.value) })}
                  placeholder="Enter amount"
                  required
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="dueDate">Due Date</Label>
                <Input
                  id="dueDate"
                  type="date"
                  value={formData.dueDate}
                  onChange={(e) => setFormData({ ...formData, dueDate: e.target.value })}
                  required
                />
              </div>
              <div className="flex justify-end space-x-2">
                <Button type="button" variant="outline" onClick={handleCloseDialog}>
                  Cancel
                </Button>
                <Button type="submit" className="bg-gradient-primary">
                  Create Record
                </Button>
              </div>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      {/* Stats Cards */}
      <div className="grid gap-4 md:grid-cols-6">
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Total Records
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{stats.total}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Paid
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-status-present">{stats.paid}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Pending
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-status-late">{stats.pending}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Overdue
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-status-absent">{stats.overdue}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Total Amount
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">${stats.totalAmount.toLocaleString()}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Collected
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-status-present">${stats.collectedAmount.toLocaleString()}</div>
          </CardContent>
        </Card>
      </div>

      {/* Fee Records Table */}
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle className="flex items-center gap-2">
              <DollarSign className="h-5 w-5" />
              Fee Records
            </CardTitle>
            <div className="flex items-center space-x-2">
              <Search className="h-4 w-4 text-muted-foreground" />
              <Input
                placeholder="Search records..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="w-64"
              />
            </div>
          </div>
        </CardHeader>
        <CardContent>
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
                    onClick={() => handleSort("class")}
                  >
                    <div className="flex items-center">
                      Class
                      {getSortIcon("class")}
                    </div>
                  </TableHead>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("feeType")}
                  >
                    <div className="flex items-center">
                      Fee Type
                      {getSortIcon("feeType")}
                    </div>
                  </TableHead>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("amount")}
                  >
                    <div className="flex items-center">
                      Amount
                      {getSortIcon("amount")}
                    </div>
                  </TableHead>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("dueDate")}
                  >
                    <div className="flex items-center">
                      Due Date
                      {getSortIcon("dueDate")}
                    </div>
                  </TableHead>
                  <TableHead 
                    className="cursor-pointer select-none hover:bg-muted/50"
                    onClick={() => handleSort("paidDate")}
                  >
                    <div className="flex items-center">
                      Paid Date
                      {getSortIcon("paidDate")}
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
                {filteredRecords.map((record) => (
                  <TableRow key={record.id}>
                    <TableCell className="font-medium">{record.studentName}</TableCell>
                    <TableCell>{record.class}</TableCell>
                    <TableCell>{record.feeType}</TableCell>
                    <TableCell className="font-mono">${record.amount}</TableCell>
                    <TableCell>
                      <div className="flex items-center space-x-2">
                        <Calendar className="h-3 w-3 text-muted-foreground" />
                        <span className="text-sm">{format(new Date(record.dueDate), 'MMM d, yyyy')}</span>
                      </div>
                    </TableCell>
                    <TableCell>
                      {record.paidDate ? (
                        <div className="flex items-center space-x-2">
                          <Calendar className="h-3 w-3 text-muted-foreground" />
                          <span className="text-sm">{format(new Date(record.paidDate), 'MMM d, yyyy')}</span>
                        </div>
                      ) : (
                        <span className="text-muted-foreground text-sm">-</span>
                      )}
                    </TableCell>
                    <TableCell>{getStatusBadge(record.status)}</TableCell>
                    <TableCell>
                      <div className="flex items-center space-x-2">
                        {record.status === 'pending' && (
                          <Button
                            variant="outline"
                            size="sm"
                            onClick={() => openPaymentDialog(record)}
                            className="bg-status-present/10 text-status-present hover:bg-status-present hover:text-white"
                          >
                            <CreditCard className="h-3 w-3 mr-1" />
                            Pay
                          </Button>
                        )}
                        <Button
                          variant="outline"
                          size="sm"
                          onClick={() => toast({
                            title: "Receipt Generated",
                            description: `Receipt for ${record.studentName} generated successfully`,
                          })}
                        >
                          <FileText className="h-3 w-3" />
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

      {/* Payment Dialog */}
      <Dialog open={isPaymentDialogOpen} onOpenChange={setIsPaymentDialogOpen}>
        <DialogContent className="sm:max-w-[400px]">
          <DialogHeader>
            <DialogTitle>Record Payment</DialogTitle>
          </DialogHeader>
          {selectedRecord && (
            <div className="space-y-4">
              <div className="bg-muted p-4 rounded-lg">
                <h4 className="font-medium">Payment Details</h4>
                <div className="mt-2 space-y-1 text-sm">
                  <p><span className="font-medium">Student:</span> {selectedRecord.studentName}</p>
                  <p><span className="font-medium">Fee Type:</span> {selectedRecord.feeType}</p>
                  <p><span className="font-medium">Amount:</span> ${selectedRecord.amount}</p>
                  <p><span className="font-medium">Due Date:</span> {format(new Date(selectedRecord.dueDate), 'MMM d, yyyy')}</p>
                </div>
              </div>
              <div className="flex justify-end space-x-2">
                <Button
                  variant="outline"
                  onClick={() => {
                    setIsPaymentDialogOpen(false);
                    setSelectedRecord(null);
                  }}
                >
                  Cancel
                </Button>
                <Button
                  onClick={() => handlePayment(selectedRecord.id)}
                  className="bg-gradient-primary"
                >
                  <CreditCard className="h-4 w-4 mr-2" />
                  Record Payment
                </Button>
              </div>
            </div>
          )}
        </DialogContent>
      </Dialog>
    </div>
  );
}