import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('FrontendPrac');

  private http = inject(HttpClient);
  // Define items as a signal initialized to an empty array
  items = signal<string[]>([]);

  ngOnInit(): void {
    this.http.get<string[]>('https://localhost:7145/api/Notes')
      .subscribe(data => {
        console.log('Setting signal data:', data);
        this.items.set(data); // Updates the view automatically
      });
  }
}
