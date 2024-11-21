import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Subject {
  sId: number;
  sCode: string;
  sTitle: string;
  sClass: number;
  stName: string;
  stSurname: string;
  sStatus: boolean;
}

@Injectable({
  providedIn: 'root'
})

export class SubjectService {
  private apiUrl = 'https://localhost:44424/subject';

  constructor(private http: HttpClient) { }

  getSubjects(): Observable<Subject[]> {
    return this.http.get<Subject[]>(this.apiUrl);
  }

  deleteSubject(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  updateSubject(id: number, subject: Subject): Observable<Subject> {
    return this.http.put<Subject>(`${this.apiUrl}/${id}`, subject);
  }

  createSubject(subject: Subject): Observable<Subject> {
    return this.http.post<Subject>('/subject', subject);
  }
}
