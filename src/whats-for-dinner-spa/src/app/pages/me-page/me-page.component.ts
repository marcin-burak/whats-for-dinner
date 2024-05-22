import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../services';

@Component({
  selector: 'app-me-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './me-page.component.html',
  styleUrl: './me-page.component.scss'
})
export class MePageComponent {

  constructor(private readonly userService: UserService) {}

  get currentUser() {
    return this.userService.currentUser;
  }

}
