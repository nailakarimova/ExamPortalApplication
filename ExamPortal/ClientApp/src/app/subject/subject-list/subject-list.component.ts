import { Component, OnInit } from '@angular/core';
import { SubjectService, Subject } from '../subject.service'

@Component({
  selector: 'app-subject-list',
  templateUrl: './subject-list.component.html',
  styleUrls: ['./subject-list.component.css']
})
export class SubjectListComponent implements OnInit {

  subjects: Subject[] = [];

  currentPage: number = 1;
  itemsPerPage: number = 5;

  get paginatedExams() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.subjects.slice(startIndex, endIndex);
  }

  setPage(page: number) {
    this.currentPage = page;
  }

  modalState: { [key: string]: boolean } = {
    editModal: false,
    deleteModal: false,
  };

  constructor(private subjectService: SubjectService) { }

  ngOnInit(): void {
    this.getSubjects();
  }

  createEmptySubject(): Subject {
    return {
      sId: 0,
      sCode: '',
      sTitle: '',
      sClass: 0,
      stName: '',
      stSurname: '',
      sStatus: true
    };
  }

  subjectToRemove: number | null = null;
  subjectToEdit: Subject = this.createEmptySubject();
  isEditMode: boolean = true;

  getSubjects(): void {
    this.subjectService.getSubjects().subscribe(subjects => {
      this.subjects = subjects;
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

  openDeleteModal(subjectId: number): void {
    const subject = this.subjects.find(sub => sub.sId === subjectId);
    this.subjectToRemove = subjectId;
    this.modalState.deleteModal = true;
    setTimeout(() => {
      this.openModal('deleteModal');
    }, 10);
  }

  confirmRemove(): void {
    if (this.subjectToRemove !== null) {
      this.removeSubject(this.subjectToRemove);
    }
    this.closeModal('deleteModal');
  }

  removeSubject(subjectId: number): void {
    this.subjectService.deleteSubject(subjectId).subscribe({
      next: (response) => {
        this.subjects = this.subjects.filter(subject => subject.sId !== subjectId);
      },
      error: (err) => {
        alert('Error deleting subject: ' + err);
      }
    });
  }

  openEditModal(subject: Subject): void {
    this.isEditMode = true;
    this.modalState.editModal = true;
    this.subjectToEdit = { ...subject };
    setTimeout(() => {
      this.openModal('editModal');
    }, 10);
  }

  submitEdit(): void {
    if (this.isEditMode) {
      this.subjectService.updateSubject(this.subjectToEdit.sId, this.subjectToEdit).subscribe({
        next: (response) => {
          const index = this.subjects.findIndex(subject => subject.sId === this.subjectToEdit.sId);
          if (index !== -1) {
            //this.subjects[index] = { ...response };
            const updatedSubject = this.subjects[index];
            updatedSubject.sCode = this.subjectToEdit.sCode;
            updatedSubject.sTitle = this.subjectToEdit.sTitle;
            updatedSubject.sClass = this.subjectToEdit.sClass;
            updatedSubject.stName = this.subjectToEdit.stName;
            updatedSubject.stSurname = this.subjectToEdit.stSurname;
            updatedSubject.sStatus = this.subjectToEdit.sStatus;
          }
          this.closeModal('editModal');
        },
        error: (err) => {
          alert('Error updating subject: ' + err.message);
        }
      });
    } else {
      this.subjectService.createSubject(this.subjectToEdit).subscribe({
        next: (response: Subject) => {
          this.subjects.push(response as Subject);
          this.closeModal('editModal');
        },
        error: (err: { message: string; }) => {
          alert('Error creating subject: ' + err.message);
        }
      });
    }
  }

  openCreateModal(): void {
    this.isEditMode = false;
    this.modalState.editModal = true;
    this.subjectToEdit = this.createEmptySubject();
    this.openModal('editModal');
  }
}
