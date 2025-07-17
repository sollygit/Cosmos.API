import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MoviesComponent } from './components/movies/movies.component';
import { MovieComponent } from './components/movie/movie.component';
import { MovieAddEditComponent } from './components/movie-add-edit/movie-add-edit.component';
import { ChartComponent } from './components/chart/chart.component';
import { BoardComponent } from './components/board/board.component';

const routes: Routes = [
  { path: '', component: MoviesComponent, pathMatch: 'full' },
  { path: 'movie/:id', component: MovieComponent },
  { path: 'add', component: MovieAddEditComponent },
  { path: 'movie/edit/:id', component: MovieAddEditComponent },
  { path: 'chart', component: ChartComponent },
  { path: 'board', component: BoardComponent },
  { path: '**', redirectTo: '/' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
