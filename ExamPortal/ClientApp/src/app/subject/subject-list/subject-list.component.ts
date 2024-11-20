import { Component, OnInit } from '@angular/core';
import { SubjectService, Subject } from '../subject.service'

@Component({
  selector: 'app-subject-list',
  templateUrl: './subject-list.component.html',
  styleUrls: ['./subject-list.component.css']
})
export class SubjectListComponent implements OnInit {

  subjects: Subject[] = [];

  modalState: { [key: string]: boolean } = {
    editModal: false,
    deleteModal: false,
  };

  subjectToRemove: number | null = null;
  subjectToEdit: any = {};

  constructor(private subjectService: SubjectService) { }

  ngOnInit(): void {
    this.getSubjects();
  }

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

  openEditModal(subject: any): void {
    this.subjectToEdit = { ...subject }; 
    this.modalState.editModal = true;
    setTimeout(() => {
      this.openModal('editModal');
    }, 10);
  }

  submitEdit(): void {
    this.subjectService.updateSubject(this.subjectToEdit.sId, this.subjectToEdit).subscribe({
      next: (response) => {
        const index = this.subjects.findIndex(subject => subject.sId === this.subjectToEdit.sId);
        if (index !== -1) {
          this.subjects[index] = { ...this.subjectToEdit }; 
        }
        this.closeModal('editModal');
      },
      error: (err) => {
        alert('Error updating subject:' + err);
      }
    });
  }

}
