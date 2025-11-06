// Mock data service for the School Management System

export interface Student {
  id: string;
  rollNumber: string;
  name: string;
  email: string;
  phone: string;
  class: string;
  section: string;
  dateOfBirth: string;
  address: string;
  parentName: string;
  parentPhone: string;
  admissionDate: string;
  status: 'active' | 'inactive';
}

export interface Employee {
  id: string;
  employeeId: string;
  name: string;
  email: string;
  phone: string;
  department: string;
  position: string;
  salary: number;
  joinDate: string;
  status: 'active' | 'inactive';
  address: string;
  qualifications: string;
}

export interface User {
  id: string;
  name: string;
  email: string;
  role: string;
  status: 'active' | 'inactive';
  lastLogin: string;
  createdAt: string;
}

export interface Role {
  id: string;
  name: string;
  description: string;
  permissions: string[];
  createdAt: string;
}

export interface Exam {
  id: string;
  name: string;
  subject: string;
  class: string;
  date: string;
  startTime: string;
  endTime: string;
  totalMarks: number;
  status: 'scheduled' | 'ongoing' | 'completed';
}

export interface FeeRecord {
  id: string;
  studentId: string;
  studentName: string;
  class: string;
  feeType: string;
  amount: number;
  dueDate: string;
  paidDate?: string;
  status: 'pending' | 'paid' | 'overdue';
}

export interface AttendanceRecord {
  id: string;
  studentId: string;
  studentName: string;
  class: string;
  date: string;
  status: 'present' | 'absent' | 'late';
  markedBy: string;
}

export interface PayrollRecord {
  id: string;
  employeeId: string;
  employeeName: string;
  month: string;
  year: number;
  basicSalary: number;
  allowances: number;
  deductions: number;
  netSalary: number;
  status: 'pending' | 'paid';
  generatedDate: string;
  paidDate?: string;
}

export interface Class {
  id: string;
  name: string;
  section: string;
  grade: string;
  classTeacher: string;
  roomNumber: string;
  capacity: number;
  currentStrength: number;
  subjects: string[];
  schedule: string;
  academicYear: string;
  status: 'active' | 'inactive';
}

export interface Department {
  id: string;
  name: string;
  code: string;
  head: string;
  description: string;
  employeeCount: number;
  budget: number;
  establishedDate: string;
  status: 'active' | 'inactive';
}

export interface ExamResult {
  id: string;
  studentId: string;
  studentName: string;
  class: string;
  examId: string;
  examName: string;
  subject: string;
  marksObtained: number;
  totalMarks: number;
  percentage: number;
  grade: string;
  remarks: string;
  publishedDate: string;
  status: 'published' | 'draft';
}

// Mock data
export const mockStudents: Student[] = [
  {
    id: '1',
    rollNumber: '2024001',
    name: 'Alice Johnson',
    email: 'alice.johnson@student.school.edu',
    phone: '+1234567890',
    class: '10',
    section: 'A',
    dateOfBirth: '2008-05-15',
    address: '123 Main St, City, State',
    parentName: 'John Johnson',
    parentPhone: '+1234567891',
    admissionDate: '2024-01-15',
    status: 'active'
  },
  {
    id: '2',
    rollNumber: '2024002',
    name: 'Bob Smith',
    email: 'bob.smith@student.school.edu',
    phone: '+1234567892',
    class: '10',
    section: 'A',
    dateOfBirth: '2008-08-22',
    address: '456 Oak Ave, City, State',
    parentName: 'Mary Smith',
    parentPhone: '+1234567893',
    admissionDate: '2024-01-16',
    status: 'active'
  },
  // Add more mock students...
];

export const mockEmployees: Employee[] = [
  {
    id: '1',
    employeeId: 'EMP001',
    name: 'Dr. Sarah Johnson',
    email: 'sarah.johnson@school.edu',
    phone: '+1234567890',
    department: 'Mathematics',
    position: 'Senior Teacher',
    salary: 75000,
    joinDate: '2020-08-15',
    status: 'active',
    address: '789 Teacher Lane, City, State',
    qualifications: 'PhD in Mathematics'
  },
  {
    id: '2',
    employeeId: 'EMP002',
    name: 'Prof. Michael Chen',
    email: 'michael.chen@school.edu',
    phone: '+1234567891',
    department: 'Science',
    position: 'Head of Department',
    salary: 85000,
    joinDate: '2019-01-10',
    status: 'active',
    address: '321 Faculty St, City, State',
    qualifications: 'MSc in Physics, B.Ed'
  }
];

export const mockUsers: User[] = [
  {
    id: '1',
    name: 'Dr. Sarah Johnson',
    email: 'admin@school.edu',
    role: 'Admin',
    status: 'active',
    lastLogin: '2024-12-27T10:30:00Z',
    createdAt: '2024-01-01T00:00:00Z'
  },
  {
    id: '2',
    name: 'Prof. Michael Chen',
    email: 'teacher@school.edu',
    role: 'Teacher',
    status: 'active',
    lastLogin: '2024-12-27T09:15:00Z',
    createdAt: '2024-01-01T00:00:00Z'
  }
];

export const mockRoles: Role[] = [
  {
    id: '1',
    name: 'Admin',
    description: 'Full system access',
    permissions: ['users.create', 'users.read', 'users.update', 'users.delete', 'students.all', 'employees.all'],
    createdAt: '2024-01-01T00:00:00Z'
  },
  {
    id: '2',
    name: 'Teacher',
    description: 'Access to students and classes',
    permissions: ['students.read', 'attendance.mark', 'exams.manage'],
    createdAt: '2024-01-01T00:00:00Z'
  }
];

export const mockExams: Exam[] = [
  {
    id: '1',
    name: 'Mid Term Math',
    subject: 'Mathematics',
    class: '10A',
    date: '2024-12-30',
    startTime: '09:00',
    endTime: '12:00',
    totalMarks: 100,
    status: 'scheduled'
  },
  {
    id: '2',
    name: 'Final Science',
    subject: 'Science',
    class: '10B',
    date: '2024-12-28',
    startTime: '10:00',
    endTime: '01:00',
    totalMarks: 100,
    status: 'completed'
  }
];

export const mockFeeRecords: FeeRecord[] = [
  {
    id: '1',
    studentId: '1',
    studentName: 'Alice Johnson',
    class: '10A',
    feeType: 'Tuition Fee',
    amount: 5000,
    dueDate: '2024-12-31',
    status: 'pending'
  },
  {
    id: '2',
    studentId: '2',
    studentName: 'Bob Smith',
    class: '10A',
    feeType: 'Library Fee',
    amount: 500,
    dueDate: '2024-12-25',
    paidDate: '2024-12-20',
    status: 'paid'
  }
];

export const mockPayrollRecords: PayrollRecord[] = [
  {
    id: '1',
    employeeId: '1',
    employeeName: 'Dr. Sarah Johnson',
    month: 'December',
    year: 2024,
    basicSalary: 75000,
    allowances: 5000,
    deductions: 8000,
    netSalary: 72000,
    status: 'paid',
    generatedDate: '2024-12-01',
    paidDate: '2024-12-05'
  },
  {
    id: '2',
    employeeId: '2',
    employeeName: 'Prof. Michael Chen',
    month: 'December',
    year: 2024,
    basicSalary: 85000,
    allowances: 7000,
    deductions: 10000,
    netSalary: 82000,
    status: 'pending',
    generatedDate: '2024-12-01'
  }
];

export const mockClasses: Class[] = [
  {
    id: '1',
    name: 'Class 10',
    section: 'A',
    grade: '10',
    classTeacher: 'Dr. Sarah Johnson',
    roomNumber: '201',
    capacity: 40,
    currentStrength: 35,
    subjects: ['Mathematics', 'Science', 'English', 'Social Studies', 'Computer Science'],
    schedule: 'Monday-Friday, 8:00 AM - 2:00 PM',
    academicYear: '2024-2025',
    status: 'active'
  },
  {
    id: '2',
    name: 'Class 10',
    section: 'B',
    grade: '10',
    classTeacher: 'Prof. Michael Chen',
    roomNumber: '202',
    capacity: 40,
    currentStrength: 38,
    subjects: ['Mathematics', 'Science', 'English', 'Social Studies', 'Physics'],
    schedule: 'Monday-Friday, 8:00 AM - 2:00 PM',
    academicYear: '2024-2025',
    status: 'active'
  },
  {
    id: '3',
    name: 'Class 9',
    section: 'A',
    grade: '9',
    classTeacher: 'Ms. Emily Rodriguez',
    roomNumber: '101',
    capacity: 35,
    currentStrength: 30,
    subjects: ['Mathematics', 'Science', 'English', 'History', 'Geography'],
    schedule: 'Monday-Friday, 8:00 AM - 2:00 PM',
    academicYear: '2024-2025',
    status: 'active'
  }
];

export const mockDepartments: Department[] = [
  {
    id: '1',
    name: 'Mathematics',
    code: 'MATH',
    head: 'Dr. Sarah Johnson',
    description: 'Mathematics department covering all levels of mathematical education',
    employeeCount: 8,
    budget: 150000,
    establishedDate: '2010-01-15',
    status: 'active'
  },
  {
    id: '2',
    name: 'Science',
    code: 'SCI',
    head: 'Prof. Michael Chen',
    description: 'Science department including Physics, Chemistry, and Biology',
    employeeCount: 12,
    budget: 250000,
    establishedDate: '2010-01-15',
    status: 'active'
  },
  {
    id: '3',
    name: 'English',
    code: 'ENG',
    head: 'Ms. Emily Rodriguez',
    description: 'English language and literature department',
    employeeCount: 6,
    budget: 100000,
    establishedDate: '2010-01-15',
    status: 'active'
  },
  {
    id: '4',
    name: 'Computer Science',
    code: 'CS',
    head: 'Mr. David Kim',
    description: 'Computer Science and IT department',
    employeeCount: 5,
    budget: 200000,
    establishedDate: '2015-08-01',
    status: 'active'
  }
];

export const mockExamResults: ExamResult[] = [
  {
    id: '1',
    studentId: '1',
    studentName: 'Alice Johnson',
    class: '10A',
    examId: '1',
    examName: 'Mid Term Math',
    subject: 'Mathematics',
    marksObtained: 85,
    totalMarks: 100,
    percentage: 85,
    grade: 'A',
    remarks: 'Excellent performance',
    publishedDate: '2024-12-15',
    status: 'published'
  },
  {
    id: '2',
    studentId: '2',
    studentName: 'Bob Smith',
    class: '10A',
    examId: '1',
    examName: 'Mid Term Math',
    subject: 'Mathematics',
    marksObtained: 78,
    totalMarks: 100,
    percentage: 78,
    grade: 'B+',
    remarks: 'Good work',
    publishedDate: '2024-12-15',
    status: 'published'
  },
  {
    id: '3',
    studentId: '1',
    studentName: 'Alice Johnson',
    class: '10A',
    examId: '2',
    examName: 'Final Science',
    subject: 'Science',
    marksObtained: 92,
    totalMarks: 100,
    percentage: 92,
    grade: 'A+',
    remarks: 'Outstanding',
    publishedDate: '2024-12-20',
    status: 'published'
  }
];

// Mock API functions
export const studentAPI = {
  getAll: () => Promise.resolve(mockStudents),
  getById: (id: string) => Promise.resolve(mockStudents.find(s => s.id === id)),
  create: (student: Omit<Student, 'id'>) => {
    const newStudent = { ...student, id: Date.now().toString() };
    mockStudents.push(newStudent);
    return Promise.resolve(newStudent);
  },
  update: (id: string, student: Partial<Student>) => {
    const index = mockStudents.findIndex(s => s.id === id);
    if (index !== -1) {
      mockStudents[index] = { ...mockStudents[index], ...student };
      return Promise.resolve(mockStudents[index]);
    }
    return Promise.reject(new Error('Student not found'));
  },
  delete: (id: string) => {
    const index = mockStudents.findIndex(s => s.id === id);
    if (index !== -1) {
      const deleted = mockStudents.splice(index, 1)[0];
      return Promise.resolve(deleted);
    }
    return Promise.reject(new Error('Student not found'));
  }
};

export const employeeAPI = {
  getAll: () => Promise.resolve(mockEmployees),
  getById: (id: string) => Promise.resolve(mockEmployees.find(e => e.id === id)),
  create: (employee: Omit<Employee, 'id'>) => {
    const newEmployee = { ...employee, id: Date.now().toString() };
    mockEmployees.push(newEmployee);
    return Promise.resolve(newEmployee);
  },
  update: (id: string, employee: Partial<Employee>) => {
    const index = mockEmployees.findIndex(e => e.id === id);
    if (index !== -1) {
      mockEmployees[index] = { ...mockEmployees[index], ...employee };
      return Promise.resolve(mockEmployees[index]);
    }
    return Promise.reject(new Error('Employee not found'));
  },
  delete: (id: string) => {
    const index = mockEmployees.findIndex(e => e.id === id);
    if (index !== -1) {
      const deleted = mockEmployees.splice(index, 1)[0];
      return Promise.resolve(deleted);
    }
    return Promise.reject(new Error('Employee not found'));
  }
};

export const userAPI = {
  getAll: () => Promise.resolve(mockUsers),
  create: (user: Omit<User, 'id'>) => {
    const newUser = { ...user, id: Date.now().toString() };
    mockUsers.push(newUser);
    return Promise.resolve(newUser);
  },
  update: (id: string, user: Partial<User>) => {
    const index = mockUsers.findIndex(u => u.id === id);
    if (index !== -1) {
      mockUsers[index] = { ...mockUsers[index], ...user };
      return Promise.resolve(mockUsers[index]);
    }
    return Promise.reject(new Error('User not found'));
  },
  delete: (id: string) => {
    const index = mockUsers.findIndex(u => u.id === id);
    if (index !== -1) {
      const deleted = mockUsers.splice(index, 1)[0];
      return Promise.resolve(deleted);
    }
    return Promise.reject(new Error('User not found'));
  }
};

export const roleAPI = {
  getAll: () => Promise.resolve(mockRoles),
  create: (role: Omit<Role, 'id'>) => {
    const newRole = { ...role, id: Date.now().toString() };
    mockRoles.push(newRole);
    return Promise.resolve(newRole);
  },
  update: (id: string, role: Partial<Role>) => {
    const index = mockRoles.findIndex(r => r.id === id);
    if (index !== -1) {
      mockRoles[index] = { ...mockRoles[index], ...role };
      return Promise.resolve(mockRoles[index]);
    }
    return Promise.reject(new Error('Role not found'));
  },
  delete: (id: string) => {
    const index = mockRoles.findIndex(r => r.id === id);
    if (index !== -1) {
      const deleted = mockRoles.splice(index, 1)[0];
      return Promise.resolve(deleted);
    }
    return Promise.reject(new Error('Role not found'));
  }
};

export const examAPI = {
  getAll: () => Promise.resolve(mockExams),
  create: (exam: Omit<Exam, 'id'>) => {
    const newExam = { ...exam, id: Date.now().toString() };
    mockExams.push(newExam);
    return Promise.resolve(newExam);
  },
  update: (id: string, exam: Partial<Exam>) => {
    const index = mockExams.findIndex(e => e.id === id);
    if (index !== -1) {
      mockExams[index] = { ...mockExams[index], ...exam };
      return Promise.resolve(mockExams[index]);
    }
    return Promise.reject(new Error('Exam not found'));
  },
  delete: (id: string) => {
    const index = mockExams.findIndex(e => e.id === id);
    if (index !== -1) {
      const deleted = mockExams.splice(index, 1)[0];
      return Promise.resolve(deleted);
    }
    return Promise.reject(new Error('Exam not found'));
  }
};

export const feeAPI = {
  getAll: () => Promise.resolve(mockFeeRecords),
  create: (fee: Omit<FeeRecord, 'id'>) => {
    const newFee = { ...fee, id: Date.now().toString() };
    mockFeeRecords.push(newFee);
    return Promise.resolve(newFee);
  },
  update: (id: string, fee: Partial<FeeRecord>) => {
    const index = mockFeeRecords.findIndex(f => f.id === id);
    if (index !== -1) {
      mockFeeRecords[index] = { ...mockFeeRecords[index], ...fee };
      return Promise.resolve(mockFeeRecords[index]);
    }
    return Promise.reject(new Error('Fee record not found'));
  }
};

export const payrollAPI = {
  getAll: () => Promise.resolve(mockPayrollRecords),
  getById: (id: string) => Promise.resolve(mockPayrollRecords.find(p => p.id === id)),
  create: (payroll: Omit<PayrollRecord, 'id'>) => {
    const newPayroll = { ...payroll, id: Date.now().toString() };
    mockPayrollRecords.push(newPayroll);
    return Promise.resolve(newPayroll);
  },
  update: (id: string, payroll: Partial<PayrollRecord>) => {
    const index = mockPayrollRecords.findIndex(p => p.id === id);
    if (index !== -1) {
      mockPayrollRecords[index] = { ...mockPayrollRecords[index], ...payroll };
      return Promise.resolve(mockPayrollRecords[index]);
    }
    return Promise.reject(new Error('Payroll record not found'));
  }
};

export const classAPI = {
  getAll: () => Promise.resolve(mockClasses),
  getById: (id: string) => Promise.resolve(mockClasses.find(c => c.id === id)),
  create: (classData: Omit<Class, 'id'>) => {
    const newClass = { ...classData, id: Date.now().toString() };
    mockClasses.push(newClass);
    return Promise.resolve(newClass);
  },
  update: (id: string, classData: Partial<Class>) => {
    const index = mockClasses.findIndex(c => c.id === id);
    if (index !== -1) {
      mockClasses[index] = { ...mockClasses[index], ...classData };
      return Promise.resolve(mockClasses[index]);
    }
    return Promise.reject(new Error('Class not found'));
  },
  delete: (id: string) => {
    const index = mockClasses.findIndex(c => c.id === id);
    if (index !== -1) {
      const deleted = mockClasses.splice(index, 1)[0];
      return Promise.resolve(deleted);
    }
    return Promise.reject(new Error('Class not found'));
  }
};

export const departmentAPI = {
  getAll: () => Promise.resolve(mockDepartments),
  getById: (id: string) => Promise.resolve(mockDepartments.find(d => d.id === id)),
  create: (dept: Omit<Department, 'id'>) => {
    const newDept = { ...dept, id: Date.now().toString() };
    mockDepartments.push(newDept);
    return Promise.resolve(newDept);
  },
  update: (id: string, dept: Partial<Department>) => {
    const index = mockDepartments.findIndex(d => d.id === id);
    if (index !== -1) {
      mockDepartments[index] = { ...mockDepartments[index], ...dept };
      return Promise.resolve(mockDepartments[index]);
    }
    return Promise.reject(new Error('Department not found'));
  },
  delete: (id: string) => {
    const index = mockDepartments.findIndex(d => d.id === id);
    if (index !== -1) {
      const deleted = mockDepartments.splice(index, 1)[0];
      return Promise.resolve(deleted);
    }
    return Promise.reject(new Error('Department not found'));
  }
};

export const examResultAPI = {
  getAll: () => Promise.resolve(mockExamResults),
  getById: (id: string) => Promise.resolve(mockExamResults.find(r => r.id === id)),
  getByStudent: (studentId: string) => Promise.resolve(mockExamResults.filter(r => r.studentId === studentId)),
  getByExam: (examId: string) => Promise.resolve(mockExamResults.filter(r => r.examId === examId)),
  create: (result: Omit<ExamResult, 'id'>) => {
    const newResult = { ...result, id: Date.now().toString() };
    mockExamResults.push(newResult);
    return Promise.resolve(newResult);
  },
  update: (id: string, result: Partial<ExamResult>) => {
    const index = mockExamResults.findIndex(r => r.id === id);
    if (index !== -1) {
      mockExamResults[index] = { ...mockExamResults[index], ...result };
      return Promise.resolve(mockExamResults[index]);
    }
    return Promise.reject(new Error('Exam result not found'));
  },
  delete: (id: string) => {
    const index = mockExamResults.findIndex(r => r.id === id);
    if (index !== -1) {
      const deleted = mockExamResults.splice(index, 1)[0];
      return Promise.resolve(deleted);
    }
    return Promise.reject(new Error('Exam result not found'));
  }
};