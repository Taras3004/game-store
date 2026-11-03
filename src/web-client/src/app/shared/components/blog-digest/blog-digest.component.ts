import { Component } from '@angular/core';

@Component({
  selector: 'blog-digest',
  standalone: true,
  templateUrl: "./blog-digest.component.html"
})
export class BlogDigestComponent {
  posts = [
    { id: 1, text: 'Proin commodo sodales arcu, a volutpat. Nulla convallis.', link: '#' },
    { id: 2, text: 'Proin commodo sodales arcu, volutpat. Nulla convallis a non.', link: '#' },
    { id: 3, text: 'Curabitur a aliquet turpis, sagittis eleifend sem.', link: '#' },
    { id: 4, text: 'Nam pellentesque finibus felis eget feugiat. Nulla a nisl non veh.', link: '#' }
  ];
}