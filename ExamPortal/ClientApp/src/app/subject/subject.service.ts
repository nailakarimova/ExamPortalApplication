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

  getSubject(id: number): Observable<Subject> {
    return this.http.get<Subject>(`${this.apiUrl}/${id}`);
  }
}
