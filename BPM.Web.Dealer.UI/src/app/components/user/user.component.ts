import { ChangeDetectorRef, Component } from '@angular/core';
import { SpinnerLoadingService } from '@app/common/services/spinner-loading-service';
import { UserService } from '@app/services/profile.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
})
export class UserComponent { 
constructor(private userService: UserService ){}



}
  
