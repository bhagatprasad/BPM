import { Component, Input, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-user-personal-info',
  imports: [],
  templateUrl: './user-personal-info.component.html',
  styleUrl: './user-personal-info.component.css',
})
export class UserPersonalInfoComponent {
  @Input() userId: string = '';

  constructor() {
    console.log('UserPersonalInfoComponent initialized with userId:', this.userId);
  }
  ngOnChanges(changes: SimpleChanges) {
    if (changes['userId']) {
      console.log('User ID changed to:', changes['userId'].currentValue);
      this.userId = changes['userId'].currentValue;
    }
  }
}
