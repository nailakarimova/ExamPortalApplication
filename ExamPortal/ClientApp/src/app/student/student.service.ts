import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Student {
  sId: number;
  sNumber: number;
  sName: string;
  sSurname: string;
  sClass: number;
  sStatus: boolean;
}

@Injectable({
  providedIn: 'root'
})

export class StudentService {
  private apiUrl = 'https://localhost:44424/student';

  constructor(private http: HttpClient) { }

  getStudents(): Observable<Student[]> {
    return this.http.get<Student[]>(this.apiUrl);
  }

  deleteStudent(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  updateStudent(id: number, student: Student): Observable<Student> {
    return this.http.put<Student>(`${this.apiUrl}/${id}`, student);
  }

  createStudent(student: Student): Observable<Student> {
    return this.http.post<Student>('/student', student);
  }
}
