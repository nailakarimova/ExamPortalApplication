import { Component, OnInit } from '@angular/core';
import { ExamService, Exam } from '../exam.service'

@Component({
  selector: 'app-exam-list',
  templateUrl: './exam-list.component.html',
  styleUrls: ['./exam-list.component.css']
})

export class ExamListComponent implements OnInit {

  exams: Exam[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 5;

  get paginatedExams() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.exams.slice(startIndex, endIndex);
  }

  setPage(page: number) {
    this.currentPage = page;
  }

  modalState: { [key: string]: boolean } = {
    editModal: false,
    deleteModal: false,
  };

  subjects: any[] = [];
  students: any[] = [];

  constructor(private examService: ExamService) { }

  ngOnInit(): void {
    this.getExams();
    this.loadSubjects();
    this.loadStudents();
  }

  loadSubjects(): void {
    this.examService.getSubjects().subscribe((subjects: any[]) => {
      this.subjects = subjects;
    });
  }

  loadStudents(): void {
    this.examService.getStudents().subscribe((students: any[]) => {
      this.students = students;
    });
  }

  createEmptyExam(): Exam {
    return {
      eId: 0,
      esCode: '',
      subjectTitle: '',
      esNumber: 0,
      studentName: '',
      studentSurname: '',
      eDate: new Date(),
      eGrade: 0,
      eStatus: true
    };
  }

  examToRemove: number | null = null;
  examToEdit: Exam = this.createEmptyExam();
  isEditMode: boolean = true;

  getExams(): void {
    this.examService.getExams().subscribe(exams => {
      this.exams = exams;
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

  openDeleteModal(examId: number): void {
    const exam = this.exams.find(e => e.eId === examId);
    this.examToRemove = examId;
    this.modalState.deleteModal = true;
    setTimeout(() => {
      this.openModal('deleteModal');
    }, 10);
  }

  confirmRemove(): void {
    if (this.examToRemove !== null) {
      this.removeExam(this.examToRemove);
    }
    this.closeModal('deleteModal');
  }

  removeExam(examId: number): void {
    this.examService.deleteExam(examId).subscribe({
      next: (response) => {
        this.exams = this.exams.filter(e => e.eId !== examId);
      },
      error: (err) => {
        alert('Error deleting exam: ' + err);
      }
    });
  }

  openEditModal(exam: Exam): void {
    this.isEditMode = true;
    this.modalState.editModal = true;
    this.examToEdit = { ...exam };
    setTimeout(() => {
      this.openModal('editModal');
    }, 10);
  }

  submitEdit(): void {
    const exam = {
      "eId": this.examToEdit.eId,
      "esCode": this.examToEdit.esCode,
      "esNumber": this.examToEdit.esNumber,
      "eDate": this.examToEdit.eDate,
      "eGrade": this.examToEdit.eGrade,
      "eStatus": this.examToEdit.eStatus
    };

    if (this.isEditMode) {
      this.examService.updateExam(exam.eId, exam).subscribe({
        next: (response) => {
          const index = this.exams.findIndex(exam => exam.eId === this.examToEdit.eId);
          if (index !== -1) {
            const updatedExam = this.exams[index];
            updatedExam.esCode = this.examToEdit.esCode;
            updatedExam.subjectTitle = this.examToEdit.subjectTitle;
            updatedExam.esNumber = this.examToEdit.esNumber;
            updatedExam.studentName = this.examToEdit.studentName;
            updatedExam.studentSurname = this.examToEdit.studentSurname;
            updatedExam.eDate = this.examToEdit.eDate;
            updatedExam.eGrade = this.examToEdit.eGrade;
            updatedExam.eStatus = this.examToEdit.eStatus;
          }
          this.closeModal('editModal');
        },
        error: (err) => {
          alert('Error updating exam: ' + err.message);
        }
      });
    } else {
      this.examService.createExam(this.examToEdit).subscribe({
        next: (response: Exam) => {
          this.exams.push(response);
          this.closeModal('editModal');
        },
        error: (err: { message: string; }) => {
          alert('Error creating exam: ' + err.message);
        }
      });
    }
  }

  openCreateModal(): void {
    this.isEditMode = false;
    this.modalState.editModal = true;
    this.examToEdit = this.createEmptyExam();
    this.openModal('editModal');
  }

  isFormValid(): boolean {
    return this.examToEdit.esCode != "" &&
      this.examToEdit.esNumber != 0 &&
      this.examToEdit.eDate != null &&
      this.examToEdit.eGrade != null &&
      (this.examToEdit.eGrade <= 5 && this.examToEdit.eGrade >= 0);
  }
}
