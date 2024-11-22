import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Exam {
  eId: number;
  esCode: string;
  subjectTitle: string;
  esNumber: number;
  studentName: string;
  studentSurname: string;
  eDate: Date;
  eGrade: number;
  eStatus: boolean;
}

@Injectable({
  providedIn: 'root'
})

export class ExamService {
  private apiUrl = 'https://localhost:44424/exam';

  constructor(private http: HttpClient) { }

  getExams(): Observable<Exam[]> {
    return this.http.get<Exam[]>(this.apiUrl);
  }

  deleteExam(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getSubjects(): Observable<any[]> {
    return this.http.get<any[]>(`https://localhost:44424/subject`);
  }

  getStudents(): Observable<any[]> {
    return this.http.get<any[]>(`https://localhost:44424/student`);
  }

  updateExam(id: number, exam: any): Observable<Exam> {
    return this.http.put<Exam>(`${this.apiUrl}/${id}`, exam);
  }

  createExam(exam: any): Observable<Exam> {
    return this.http.post<Exam>('/exam', exam);
  }
}
