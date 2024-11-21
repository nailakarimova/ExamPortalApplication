import { Component, OnInit } from '@angular/core';
import { StudentService, Student } from '../student.service'

@Component({
  selector: 'app-student-list',
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.css']
})
export class StudentListComponent implements OnInit {

  students: Student[] = [];

  modalState: { [key: string]: boolean } = {
    editModal: false,
    deleteModal: false,
  };

  constructor(private studentService: StudentService) { }

  ngOnInit(): void {
    this.getStudents();
  }

  createEmptyStudent(): Student {
    return {
      sId: 0,
      sNumber: 0,
      sName: '',
      sSurname: '',
      sClass: 0,
      sStatus: true
    };
  }

  studentToRemove: number | null = null;
  studentToEdit: Student = this.createEmptyStudent();
  isEditMode: boolean = true;

  getStudents(): void {
    this.studentService.getStudents().subscribe(students => {
      this.students = students;
    });
  }

  closeModal(modalId: string): void {
    if (this.modalState[modalId] !== undefined) {
      this.modalState[modalId] = false;
    }

    const modal = document.getElementById(modalId);
    if (modal) {
      modal.classList.remove('show');
      setTimeout(() => {
        modal.style.display = 'none';
      }, 300);
    }
  }

  openModal(modalId: string): void {
    const modal = document.getElementById(modalId);
    if (modal) {
      modal.style.display = 'flex';
      setTimeout(() => modal.classList.add('show'), 10);
    }
  }

  openDeleteModal(studentId: number): void {
    const student = this.students.find(st => st.sId === studentId);
    this.studentToRemove = studentId;
    this.modalState.deleteModal = true;
    setTimeout(() => {
      this.openModal('deleteModal');
    }, 10);
  }

  confirmRemove(): void {
    if (this.studentToRemove !== null) {
      this.removeStudent(this.studentToRemove);
    }
    this.closeModal('deleteModal');
  }

  removeStudent(studentId: number): void {
    this.studentService.deleteStudent(studentId).subscribe({
      next: (response) => {
        this.students = this.students.filter(student => student.sId !== studentId);
      },
      error: (err) => {
        alert('Error deleting student: ' + err);
      }
    });
  }

  openEditModal(student: Student): void {
    this.isEditMode = true;
    this.modalState.editModal = true;
    this.studentToEdit = { ...student };
    setTimeout(() => {
      this.openModal('editModal');
    }, 10);
  }

  submitEdit(): void {
    if (this.isEditMode) {
      this.studentService.updateStudent(this.studentToEdit.sId, this.studentToEdit).subscribe({
        next: (response) => {
          const index = this.students.findIndex(student => student.sId === this.studentToEdit.sId);
          if (index !== -1) {
            //this.students[index] = { ...response };
            const updatedStudent = this.students[index];
            updatedStudent.sNumber = this.studentToEdit.sNumber;
            updatedStudent.sName = this.studentToEdit.sName;
            updatedStudent.sSurname = this.studentToEdit.sSurname;
            updatedStudent.sClass = this.studentToEdit.sClass;
            updatedStudent.sStatus = this.studentToEdit.sStatus;
          }
          this.closeModal('editModal');
        },
        error: (err) => {
          alert('Error updating student: ' + err.message);
        }
      });
    } else {
      this.studentService.createStudent(this.studentToEdit).subscribe({
        next: (response: Student) => {
          this.students.push(response as Student);
          this.closeModal('editModal');
        },
        error: (err: { message: string; }) => {
          alert('Error creating student: ' + err.message);
        }
      });
    }
  }

  trackByStudentId(index: number, student: Student): number {
    return student.sId;
  }

  openCreateModal(): void {
    this.isEditMode = false;
    this.modalState.editModal = true;
    this.studentToEdit = this.createEmptyStudent();
    this.openModal('editModal');
  }
}
