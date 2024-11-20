import { Component, OnInit } from '@angular/core';
import { SubjectService, Subject } from '../subject.service'

@Component({
  selector: 'app-subject-list',
  templateUrl: './subject-list.component.html',
  styleUrls: ['./subject-list.component.css']
})
export class SubjectListComponent implements OnInit {

  subjects: Subject[] = [];

  constructor(private subjectService: SubjectService) { }

  ngOnInit(): void {
    this.getSubjects();
  }

  getSubjects(): void {
    this.subjectService.getSubjects().subscribe(subjects => {
      this.subjects = subjects;
    });
  }

}
