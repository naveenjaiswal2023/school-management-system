/**
 * Exam Results Module
 * Record and manage student exam results with grade calculation
 * Features: Add results, View by student/exam, Search, Export, Grade calculation
 */

import { useState, useEffect } from "react";
import { Plus, Search, Edit, Trash2, Eye, Download, TrendingUp, Award } from "lucide-react";
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
import {
  examResultAPI,
  studentAPI,
  examAPI,
  ExamResult,
  Student,
  Exam,
} from "@/services/mockData";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";

export default function ExamResults() {
  const [results, setResults] = useState<ExamResult[]>([]);
  const [students, setStudents] = useState<Student[]>([]);
  const [exams, setExams] = useState<Exam[]>([]);
  const [filteredResults, setFilteredResults] = useState<ExamResult[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [statusFilter, setStatusFilter] = useState<string>("all");
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [isViewDialogOpen, setIsViewDialogOpen] = useState(false);
  const [selectedResult, setSelectedResult] = useState<ExamResult | null>(null);
  const [formData, setFormData] = useState<Partial<ExamResult>>({
    studentId: "",
    studentName: "",
    class: "",
    examId: "",
    examName: "",
    subject: "",
    marksObtained: 0,
    totalMarks: 100,
    percentage: 0,
    grade: "",
    remarks: "",
    publishedDate: new Date().toISOString().split('T')[0],
    status: "draft",
  });

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    let filtered = results.filter(
      (result) =>
        result.studentName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        result.examName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        result.subject.toLowerCase().includes(searchTerm.toLowerCase())
    );

    if (statusFilter !== "all") {
      filtered = filtered.filter((result) => result.status === statusFilter);
    }

    setFilteredResults(filtered);
  }, [searchTerm, statusFilter, results]);

  useEffect(() => {
    // Auto-calculate percentage and grade
    if (formData.marksObtained !== undefined && formData.totalMarks) {
      const percentage = (formData.marksObtained / formData.totalMarks) * 100;
      const grade = calculateGrade(percentage);
      setFormData(prev => ({ ...prev, percentage, grade }));
    }
  }, [formData.marksObtained, formData.totalMarks]);

  const calculateGrade = (percentage: number): string => {
    if (percentage >= 90) return "A+";
    if (percentage >= 80) return "A";
    if (percentage >= 70) return "B+";
    if (percentage >= 60) return "B";
    if (percentage >= 50) return "C";
    if (percentage >= 40) return "D";
    return "F";
  };

  const loadData = async () => {
    try {
      const [resultsData, studentsData, examsData] = await Promise.all([
        examResultAPI.getAll(),
        studentAPI.getAll(),
        examAPI.getAll(),
      ]);
      setResults(resultsData);
      setFilteredResults(resultsData);
      setStudents(studentsData);
      setExams(examsData);
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to load exam results",
        variant: "destructive",
      });
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      if (selectedResult) {
        await examResultAPI.update(selectedResult.id, formData);
        toast({
          title: "Success",
          description: "Result updated successfully",
        });
      } else {
        await examResultAPI.create(formData as Omit<ExamResult, "id">);
        toast({
          title: "Success",
          description: "Result added successfully",
        });
      }
      loadData();
      resetForm();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to save result",
        variant: "destructive",
      });
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Are you sure you want to delete this result?")) return;

    try {
      await examResultAPI.delete(id);
      toast({
        title: "Success",
        description: "Result deleted successfully",
      });
      loadData();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete result",
        variant: "destructive",
      });
    }
  };

  const handlePublish = async (id: string) => {
    try {
      await examResultAPI.update(id, {
        status: "published",
        publishedDate: new Date().toISOString().split('T')[0],
      });
      toast({
        title: "Success",
        description: "Result published successfully",
      });
      loadData();
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to publish result",
        variant: "destructive",
      });
    }
  };

  const handleEdit = (result: ExamResult) => {
    setSelectedResult(result);
    setFormData(result);
    setIsDialogOpen(true);
  };

  const handleView = (result: ExamResult) => {
    setSelectedResult(result);
    setIsViewDialogOpen(true);
  };

  const handleStudentChange = (studentId: string) => {
    const student = students.find(s => s.id === studentId);
    if (student) {
      setFormData({
        ...formData,
        studentId: student.id,
        studentName: student.name,
        class: `${student.class}${student.section}`,
      });
    }
  };

  const handleExamChange = (examId: string) => {
    const exam = exams.find(e => e.id === examId);
    if (exam) {
      setFormData({
        ...formData,
        examId: exam.id,
        examName: exam.name,
        subject: exam.subject,
        totalMarks: exam.totalMarks,
      });
    }
  };

  const handleExportResults = () => {
    toast({
      title: "Export Started",
      description: "Results are being exported to CSV",
    });
  };

  const resetForm = () => {
    setFormData({
      studentId: "",
      studentName: "",
      class: "",
      examId: "",
      examName: "",
      subject: "",
      marksObtained: 0,
      totalMarks: 100,
      percentage: 0,
      grade: "",
      remarks: "",
      publishedDate: new Date().toISOString().split('T')[0],
      status: "draft",
    });
    setSelectedResult(null);
    setIsDialogOpen(false);
  };

  const avgPercentage = results.length > 0
    ? results.reduce((sum, r) => sum + r.percentage, 0) / results.length
    : 0;
  const passCount = results.filter(r => r.percentage >= 40).length;
  const passRate = results.length > 0 ? (passCount / results.length) * 100 : 0;

  return (
    <div className="p-6 space-y-6">
      {/* Header Section */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold bg-gradient-primary bg-clip-text text-transparent">
            Exam Results
          </h1>
          <p className="text-muted-foreground mt-1">
            Record and manage student examination results
          </p>
        </div>
        <div className="flex gap-2">
          <Button variant="outline" onClick={handleExportResults}>
            <Download className="mr-2 h-4 w-4" />
            Export Results
          </Button>
          <Button onClick={() => setIsDialogOpen(true)} className="btn-professional">
            <Plus className="mr-2 h-4 w-4" />
            Add Result
          </Button>
        </div>
      </div>

      {/* Search and Filter */}
      <Card className="p-4 card-gradient glow-on-hover">
        <div className="flex gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground h-4 w-4" />
            <Input
              placeholder="Search by student, exam, or subject..."
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
              <SelectItem value="published">Published</SelectItem>
              <SelectItem value="draft">Draft</SelectItem>
            </SelectContent>
          </Select>
        </div>
      </Card>

      {/* Statistics Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Total Results</p>
              <p className="text-2xl font-bold">{results.length}</p>
            </div>
            <Award className="h-8 w-8 text-primary" />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Avg. Percentage</p>
              <p className="text-2xl font-bold">{avgPercentage.toFixed(1)}%</p>
            </div>
            <TrendingUp className="h-8 w-8" style={{ color: 'hsl(var(--education-blue))' }} />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Pass Rate</p>
              <p className="text-2xl font-bold">{passRate.toFixed(1)}%</p>
            </div>
            <Award className="h-8 w-8" style={{ color: 'hsl(var(--education-green))' }} />
          </div>
        </Card>
        <Card className="p-4 card-gradient glow-on-hover">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-muted-foreground">Published</p>
              <p className="text-2xl font-bold">
                {results.filter(r => r.status === 'published').length}
              </p>
            </div>
            <Award className="h-8 w-8" style={{ color: 'hsl(var(--education-orange))' }} />
          </div>
        </Card>
      </div>

      {/* Results Table */}
      <Tabs defaultValue="all">
        <TabsList>
          <TabsTrigger value="all">All Results</TabsTrigger>
          <TabsTrigger value="published">Published</TabsTrigger>
          <TabsTrigger value="draft">Draft</TabsTrigger>
        </TabsList>

        <TabsContent value="all" className="mt-4">
          <Card className="card-gradient">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Student</TableHead>
                  <TableHead>Class</TableHead>
                  <TableHead>Exam</TableHead>
                  <TableHead>Subject</TableHead>
                  <TableHead>Marks</TableHead>
                  <TableHead>Percentage</TableHead>
                  <TableHead>Grade</TableHead>
                  <TableHead>Status</TableHead>
                  <TableHead className="text-right">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredResults.map((result) => (
                  <TableRow key={result.id}>
                    <TableCell className="font-medium">{result.studentName}</TableCell>
                    <TableCell>{result.class}</TableCell>
                    <TableCell>{result.examName}</TableCell>
                    <TableCell>{result.subject}</TableCell>
                    <TableCell>
                      {result.marksObtained}/{result.totalMarks}
                    </TableCell>
                    <TableCell>{result.percentage.toFixed(1)}%</TableCell>
                    <TableCell>
                      <Badge
                        variant={
                          result.grade.startsWith('A')
                            ? 'default'
                            : result.grade === 'F'
                            ? 'destructive'
                            : 'secondary'
                        }
                      >
                        {result.grade}
                      </Badge>
                    </TableCell>
                    <TableCell>
                      <Badge variant={result.status === 'published' ? 'default' : 'secondary'}>
                        {result.status}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-right">
                      <div className="flex justify-end gap-2">
                        <Button variant="ghost" size="sm" onClick={() => handleView(result)}>
                          <Eye className="h-4 w-4" />
                        </Button>
                        <Button variant="ghost" size="sm" onClick={() => handleEdit(result)}>
                          <Edit className="h-4 w-4" />
                        </Button>
                        {result.status === 'draft' && (
                          <Button
                            variant="ghost"
                            size="sm"
                            onClick={() => handlePublish(result.id)}
                          >
                            Publish
                          </Button>
                        )}
                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => handleDelete(result.id)}
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
        </TabsContent>

        <TabsContent value="published" className="mt-4">
          <Card className="card-gradient">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Student</TableHead>
                  <TableHead>Class</TableHead>
                  <TableHead>Exam</TableHead>
                  <TableHead>Subject</TableHead>
                  <TableHead>Marks</TableHead>
                  <TableHead>Grade</TableHead>
                  <TableHead className="text-right">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredResults
                  .filter((r) => r.status === 'published')
                  .map((result) => (
                    <TableRow key={result.id}>
                      <TableCell className="font-medium">{result.studentName}</TableCell>
                      <TableCell>{result.class}</TableCell>
                      <TableCell>{result.examName}</TableCell>
                      <TableCell>{result.subject}</TableCell>
                      <TableCell>
                        {result.marksObtained}/{result.totalMarks}
                      </TableCell>
                      <TableCell>
                        <Badge variant="default">{result.grade}</Badge>
                      </TableCell>
                      <TableCell className="text-right">
                        <Button variant="ghost" size="sm" onClick={() => handleView(result)}>
                          <Eye className="h-4 w-4" />
                        </Button>
                      </TableCell>
                    </TableRow>
                  ))}
              </TableBody>
            </Table>
          </Card>
        </TabsContent>

        <TabsContent value="draft" className="mt-4">
          <Card className="card-gradient">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Student</TableHead>
                  <TableHead>Exam</TableHead>
                  <TableHead>Marks</TableHead>
                  <TableHead>Grade</TableHead>
                  <TableHead className="text-right">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredResults
                  .filter((r) => r.status === 'draft')
                  .map((result) => (
                    <TableRow key={result.id}>
                      <TableCell className="font-medium">{result.studentName}</TableCell>
                      <TableCell>{result.examName}</TableCell>
                      <TableCell>
                        {result.marksObtained}/{result.totalMarks}
                      </TableCell>
                      <TableCell>
                        <Badge variant="secondary">{result.grade}</Badge>
                      </TableCell>
                      <TableCell className="text-right">
                        <div className="flex justify-end gap-2">
                          <Button variant="ghost" size="sm" onClick={() => handleEdit(result)}>
                            <Edit className="h-4 w-4" />
                          </Button>
                          <Button
                            variant="ghost"
                            size="sm"
                            onClick={() => handlePublish(result.id)}
                          >
                            Publish
                          </Button>
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
              </TableBody>
            </Table>
          </Card>
        </TabsContent>
      </Tabs>

      {/* Add/Edit Dialog */}
      <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
        <DialogContent className="max-w-2xl">
          <DialogHeader>
            <DialogTitle>
              {selectedResult ? 'Edit Result' : 'Add New Result'}
            </DialogTitle>
            <DialogDescription>
              {selectedResult ? 'Update exam result' : 'Record a new exam result'}
            </DialogDescription>
          </DialogHeader>
          <form onSubmit={handleSubmit}>
            <div className="grid gap-4 py-4">
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label htmlFor="student">Student</Label>
                  <Select
                    value={formData.studentId}
                    onValueChange={handleStudentChange}
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Select student" />
                    </SelectTrigger>
                    <SelectContent>
                      {students.map((student) => (
                        <SelectItem key={student.id} value={student.id}>
                          {student.name} - {student.class}{student.section}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
                <div>
                  <Label htmlFor="exam">Exam</Label>
                  <Select value={formData.examId} onValueChange={handleExamChange}>
                    <SelectTrigger>
                      <SelectValue placeholder="Select exam" />
                    </SelectTrigger>
                    <SelectContent>
                      {exams.map((exam) => (
                        <SelectItem key={exam.id} value={exam.id}>
                          {exam.name} - {exam.subject}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
              </div>

              <div className="grid grid-cols-3 gap-4">
                <div>
                  <Label htmlFor="marksObtained">Marks Obtained</Label>
                  <Input
                    id="marksObtained"
                    type="number"
                    value={formData.marksObtained}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        marksObtained: parseFloat(e.target.value) || 0,
                      })
                    }
                    required
                  />
                </div>
                <div>
                  <Label htmlFor="totalMarks">Total Marks</Label>
                  <Input
                    id="totalMarks"
                    type="number"
                    value={formData.totalMarks}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        totalMarks: parseFloat(e.target.value) || 100,
                      })
                    }
                    required
                  />
                </div>
                <div>
                  <Label>Percentage</Label>
                  <Input value={`${formData.percentage?.toFixed(1)}%`} disabled />
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label>Grade</Label>
                  <Input value={formData.grade} disabled />
                </div>
                <div>
                  <Label htmlFor="status">Status</Label>
                  <Select
                    value={formData.status}
                    onValueChange={(value: 'published' | 'draft') =>
                      setFormData({ ...formData, status: value })
                    }
                  >
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="draft">Draft</SelectItem>
                      <SelectItem value="published">Published</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>

              <div>
                <Label htmlFor="remarks">Remarks</Label>
                <Textarea
                  id="remarks"
                  value={formData.remarks}
                  onChange={(e) =>
                    setFormData({ ...formData, remarks: e.target.value })
                  }
                  rows={3}
                />
              </div>
            </div>
            <DialogFooter>
              <Button type="button" variant="outline" onClick={resetForm}>
                Cancel
              </Button>
              <Button type="submit" className="btn-professional">
                {selectedResult ? 'Update' : 'Add Result'}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>

      {/* View Dialog */}
      <Dialog open={isViewDialogOpen} onOpenChange={setIsViewDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Result Details</DialogTitle>
          </DialogHeader>
          {selectedResult && (
            <div className="space-y-4">
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label className="text-muted-foreground">Student Name</Label>
                  <p className="font-medium">{selectedResult.studentName}</p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Class</Label>
                  <p className="font-medium">{selectedResult.class}</p>
                </div>
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label className="text-muted-foreground">Exam Name</Label>
                  <p className="font-medium">{selectedResult.examName}</p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Subject</Label>
                  <p className="font-medium">{selectedResult.subject}</p>
                </div>
              </div>
              <div className="p-4 bg-muted rounded-lg space-y-2">
                <div className="flex justify-between">
                  <span>Marks Obtained</span>
                  <span className="font-medium">{selectedResult.marksObtained}</span>
                </div>
                <div className="flex justify-between">
                  <span>Total Marks</span>
                  <span className="font-medium">{selectedResult.totalMarks}</span>
                </div>
                <div className="flex justify-between pt-2 border-t">
                  <span>Percentage</span>
                  <span className="font-bold">{selectedResult.percentage.toFixed(1)}%</span>
                </div>
                <div className="flex justify-between">
                  <span>Grade</span>
                  <Badge variant="default">{selectedResult.grade}</Badge>
                </div>
              </div>
              {selectedResult.remarks && (
                <div>
                  <Label className="text-muted-foreground">Remarks</Label>
                  <p className="text-sm mt-1">{selectedResult.remarks}</p>
                </div>
              )}
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label className="text-muted-foreground">Published Date</Label>
                  <p className="font-medium">
                    {new Date(selectedResult.publishedDate).toLocaleDateString()}
                  </p>
                </div>
                <div>
                  <Label className="text-muted-foreground">Status</Label>
                  <Badge variant={selectedResult.status === 'published' ? 'default' : 'secondary'}>
                    {selectedResult.status}
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
