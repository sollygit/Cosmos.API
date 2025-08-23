import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { AppRoutingModule } from './app-routing.module';
import { ChartsModule, ThemeService } from 'ng2-charts';
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

@NgModule({
  declarations: [
    AppComponent,
    MoviesComponent,
    MovieComponent,
    MovieAddEditComponent,
    ChartComponent,
    BoardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatSelectModule,
    MatIconModule,
    MatProgressBarModule,
    MatSlideToggleModule,
    HttpClientModule,
    AppRoutingModule,
    ChartsModule,
    ReactiveFormsModule
  ],
  providers: [
    SessionStorageService,
    MovieService,
    ChartService,
    MovieSignalRService,
    ChartSignalRService,
    BoardSignalRService,
    ThemeService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
