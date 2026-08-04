import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-personal-info',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-personal-info.component.html',
  styleUrl: './user-personal-info.component.css',
})
export class UserPersonalInfoComponent {
  @Input() userId: string = '';
  @Input() userPersionalSection: any = {};
  @Input() isUserEditing: boolean = false;
  @Input() errorMsz: string = '';

  @Output() Edit = new EventEmitter<void>();
  @Output() save = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();


  constructor() {
    console.log('UserPersonalInfoComponent initialized with userId:', this.userId);
    console.log('UserPersonalInfoComponent initialized with userPersionalSection:', this.userPersionalSection);
    console.log('UserPersonalInfoComponent initialized with isUserEditing:', this.isUserEditing);
    console.log('UserPersonalInfoComponent initialized with errorMsz:', this.errorMsz);
    console.log('UserPersonalInfoComponent initialized with Edit event emitter:', this.Edit);
    console.log('UserPersonalInfoComponent initialized with save event emitter:', this.save);
    console.log('UserPersonalInfoComponent initialized with cancel event emitter:', this.cancel);
  }
  ngOnChanges(changes: SimpleChanges) {
    if (changes['userId']) {
      console.log('User ID changed to:', changes['userId'].currentValue);
      this.userId = changes['userId'].currentValue;
    }
    if (changes['userPersionalSection']) {
      console.log('User Data:', changes['userPersionalSection'].currentValue);
      this.userPersionalSection = changes['userPersionalSection'].currentValue;
    }
    if(changes['isUserEditing'])
    {
      console.log('isUserEditing changed to:', changes['isUserEditing'].currentValue);
      this.isUserEditing = changes['isUserEditing'].currentValue;
    }
    if(changes['errorMsz'])
    {
      console.log('errorMsz changed to:', changes['errorMsz'].currentValue);
      this.errorMsz = changes['errorMessage'].currentValue;
    }
    if(changes['Edit'])
    {
      console.log('Edit event emitter changed to:', changes['Edit'].currentValue);
      this.Edit = changes['Edit'].currentValue;
    }
    if(changes['save'])
    {
      console.log('save event emitter changed to:', changes['save'].currentValue);
      this.save = changes['save'].currentValue;
    }
    if(changes['cancel'])
    {
      console.log('cancel event emitter changed to:', changes['cancel'].currentValue);
      this.cancel = changes['cancel'].currentValue;
    }

    
  }
}

