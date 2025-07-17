import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MovieService } from '../../services/movie.service';
import { Movie } from '../../models/movie';
import { Constants } from 'src/app/services/constants';
import { fadeInOut } from '../../services/animations';

@Component({
  selector: 'app-movie-add-edit',
  templateUrl: './movie-add-edit.component.html',
  styleUrls: ['./movie-add-edit.component.css'],
  animations: [fadeInOut]
})
export class MovieAddEditComponent implements OnInit {
  form: FormGroup;
  actionType: string;
  id: string;
  errorMessage: any;
  existing: Movie;
  genreAll: string[] = Constants.GENRE;

  constructor(
    private movieService: MovieService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router) {

    const id = 'id';
    this.actionType = 'New';
    if (this.route.snapshot.params[id]) {
      this.id = this.route.snapshot.params[id];
    }
    this.form = this.formBuilder.group(
      {
        id: '',
        movieID: ['', [Validators.required]],
        title: ['', [Validators.required]],
        price: ['', [Validators.required]],
        genre: [],
        isActive: [true]
      }
    );
  }

  ngOnInit() {
    if (this.id) {
      this.actionType = 'Edit';
      this.movieService.get(this.id)
        .subscribe(data => (
          this.existing = data,
          this.form.controls['movieID'].setValue(data.movieID),
          this.form.controls['title'].setValue(data.title),
          this.form.controls['price'].setValue(data.price),
          this.form.controls['genre'].setValue(data.genre),
          this.form.controls['isActive'].setValue(data.isActive)
        ));
    }
  }

  save() {
    if (!this.form.valid) {
      return;
    }

    if (this.actionType === 'New') {
      const movie: Movie = {
        movieID: this.form.get('movieID').value,
        title: this.form.get('title').value,
        price: 1000.00,
        genre: this.form.get('genre').value || [],
        releaseDate: new Date(),
        year: new Date().getFullYear().toString(),
        points: 0,
        isActive: this.form.get('isActive').value
      };

      this.movieService.save(movie)
        .subscribe((data: Movie) => {
          this.router.navigate(['/']);
        });
    }

    if (this.actionType === 'Edit') {
      const movie: Movie = {
        id: this.existing.id,
        movieID: this.form.get('movieID').value,
        title: this.form.get('title').value,
        price: this.form.get('price').value,
        releaseDate: this.existing.releaseDate,
        year: this.existing.year,
        poster: this.existing.poster,
        points : this.existing.points,
        genre: this.form.get('genre').value,
        isActive: this.form.get('isActive').value
      };
      this.movieService.update(movie.id, movie)
        .subscribe((data) => {
          this.router.navigate(['/']);
        });
    }
  }

  get movieID() { return this.form.get('movieID'); }
  get title() { return this.form.get('title'); }
  get price() { return this.form.get('price'); }
  get genre() { return this.form.get('genre'); }
  get isActive() { return this.form.get('isActive'); }
}
