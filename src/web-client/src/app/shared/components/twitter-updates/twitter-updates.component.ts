import { Component } from "@angular/core";

@Component({
  selector: 'twitter-updates',
  standalone: true,
  templateUrl: "./twitter-updates.component.html"
})
export class TwitterUpdatesComponent {
  tweets = [
    { id: 1, title: 'Devil May Cry 5', url: '#', urlText: 'http://goo.gl/ejVm', time: 'about 1 hour ago' },
    { id: 2, title: 'Dead or Alive 6 Special Edition', url: '#', urlText: 'http://goo.gl/ejVm', time: 'about 1 hour ago' },
    { id: 3, title: 'Resident Evil 2 Deluxe Edition', url: '#', urlText: 'http://goo.gl/ejVm', time: 'about 1 hour ago' }
  ];
}