import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnectionState } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { Movie } from '../../models/movie';

@Injectable()
export class MovieSignalRService {
  private movieUrl: string;
  private hubConnection: signalR.HubConnection;
  public data: Movie[];

  public get isConnected(): boolean {
    return this.hubConnection
    && this.hubConnection.state === HubConnectionState.Connected;
  }

  constructor() {
    this.movieUrl = `${environment.baseUrl}/movie`;
  }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Debug)
      .withUrl(this.movieUrl)
      .build();
    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started');
      })
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  public addSendMoviesListener = (isActive: boolean) => {
    this.hubConnection.on('sendMovies', (data: Movie[]) => {
      this.data = data.filter(o => o.isActive === isActive);
      console.log(data);
    });
  }

}
