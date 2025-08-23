import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { SessionStorageService } from 'src/app/services/sessionStorage.service';
import { MovieSignalRService } from 'src/app/services/signal-r/movie.signal-r.service';
import { MovieService } from '../../services/movie.service';
import { Movie } from '../../models/movie';
import { fadeInOut } from '../../services/animations';
import { DBkeys } from 'src/app/services/db-keys';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css'],
  animations: [fadeInOut]
})
export class MoviesComponent implements OnInit {
  movieUrl = `${environment.baseUrl}/api/movie`;
  active = true;

  constructor(
    private sessionStorage: SessionStorageService,
    private movieService: MovieService,
    private signalRService: MovieSignalRService,
    private http: HttpClient) { }

  ngOnInit() {
    if (!this.signalRService.isConnected) {
      this.startConnection();
    }
  }

  startConnection() {
    this.signalRService.startConnection();
    this.signalRService.addSendMoviesListener(this.isActive);
    this.startHttpRequest();
  }

  startHttpRequest = () => {
    this.http.get(this.movieUrl)
      .subscribe(res => {
        console.log(res);
        this.getMovies();
      });
  }

  getMovies() {
    this.movieService.getAll()
      .subscribe(movies => {
        this.movies = movies.filter(c => c.isActive === this.isActive);
      });
  }

  delete(id: string) {
    const result = confirm('Are you sure?');
    if (result) {
      this.movieService.delete(id).subscribe(() => {
        this.getMovies();
      });
    }
  }

  set movies(value: Movie[]) {
    this.signalRService.data = value;
  }
  get movies(): Movie[] {
    return this.signalRService.data;
  }
  set isActive(value: boolean) {
    this.active = value;
    this.sessionStorage.set(DBkeys.IS_ACTIVE, value);
  }
  get isActive(): boolean {
    return this.sessionStorage.exists(DBkeys.IS_ACTIVE) ?
    this.sessionStorage.get(DBkeys.IS_ACTIVE) : this.active;
  }

}
