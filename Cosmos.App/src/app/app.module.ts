import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { NgChartsModule, ThemeService } from 'ng2-charts';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { SessionStorageService } from './services/sessionStorage.service';
import { MovieService } from './services/movie.service';
import { ChartService } from './services/chart.service';
import { MovieSignalRService } from './services/signal-r/movie.signal-r.service';
import { ChartSignalRService } from './services/signal-r/chart.signal-r.service';
import { BoardSignalRService } from './services/signal-r/board.signal-r.service';
import { AppComponent } from './app.component';
import { MoviesComponent } from './components/movies/movies.component';
import { MovieComponent } from './components/movie/movie.component';
import { MovieAddEditComponent } from './components/movie-add-edit/movie-add-edit.component';
import { ChartComponent } from './components/chart/chart.component';
import { BoardComponent } from './components/board/board.component';
import { CommonModule, JsonPipe, DatePipe, DecimalPipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { appRoutes } from './app-routing.module';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatCheckboxModule,
    MatSelectModule,
    MatIconModule,
    MatProgressBarModule,
    MatSlideToggleModule,
    CommonModule,
    NgChartsModule,
    RouterModule.forRoot(appRoutes)
  ],
  declarations: [
    AppComponent,
    MoviesComponent,
    MovieComponent,
    MovieAddEditComponent,
    ChartComponent,
    BoardComponent
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    SessionStorageService,
    MovieService,
    ChartService,
    MovieSignalRService,
    ChartSignalRService,
    BoardSignalRService,
    ThemeService,
    JsonPipe,
    DatePipe,
    DecimalPipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
