export class Movie {
  id?: string;
  movieID: string;
  title: string;
  releaseDate?: Date = new Date();
  year: string;
  poster?: string;
  price: number;
  points: number;
  genre: string[];
  isActive?: true;
}
