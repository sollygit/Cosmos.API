import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Movie } from '../models/movie';

@Injectable()
export class MovieService {
  getAllUrl: string;
  getUrl: string;
  saveUrl: string;
  updateUrl: string;
  deleteUrl: string;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  constructor(private http: HttpClient) {
      this.getAllUrl = `${environment.baseUrl}/api/Movie/All`;
      this.getUrl = `${environment.baseUrl}/api/Movie/Get`;
      this.saveUrl = `${environment.baseUrl}/api/Movie/Create`;
      this.updateUrl = `${environment.baseUrl}/api/Movie/Update`;
      this.deleteUrl = `${environment.baseUrl}/api/Movie`;
  }

  getAll(): Observable<Movie[]> {
    const result = this.http.get<Movie[]>(this.getAllUrl);
    return result.pipe(map(response => {
      if (response == null) {
        catchError(this.errorHandler);
      }
      return response;
    }));
  }

  get(id: string): Observable<Movie> {
      return this.http.get<Movie>(`${this.getUrl}/${id}`)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  save(item: Movie): Observable<Movie> {
      return this.http.post<Movie>(this.saveUrl, JSON.stringify(item), this.httpOptions)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  update(id: string, item: Movie): Observable<Movie> {
      return this.http.put<Movie>(`${this.updateUrl}/${id}`, JSON.stringify(item), this.httpOptions)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  delete(id: string): Observable<Movie> {
      return this.http.delete<Movie>(`${this.deleteUrl}/${id}`)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  errorHandler(error: any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }
}
