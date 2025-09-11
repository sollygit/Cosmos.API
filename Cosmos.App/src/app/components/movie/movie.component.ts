import { Component, OnInit } from '@angular/core';
import { JsonPipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { MovieService } from '../../services/movie.service';
import { Movie } from '../../models/movie';
import { fadeInOut } from '../../services/animations';

@Component({
  selector: 'app-movie',
  standalone: false,
  providers: [JsonPipe],
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.css'],
  animations: [fadeInOut]
})
export class MovieComponent implements OnInit {
  movie: Movie;
  id: string;

  constructor(private movieService: MovieService, private route: ActivatedRoute) {
    const id = 'id';
    if (this.route.snapshot.params[id]) {
      this.id = this.route.snapshot.params[id];
    }
  }

  ngOnInit() {
    this.loadMovie();
  }

  loadMovie() {
    this.movieService.get(this.id)
      .subscribe(movie => {
        this.movie = movie;
      });
  }

}
