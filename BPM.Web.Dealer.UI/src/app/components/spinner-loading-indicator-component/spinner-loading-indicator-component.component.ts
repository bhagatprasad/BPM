import { Component, ContentChild, Input, TemplateRef, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { Router, RouteConfigLoadStart, RouteConfigLoadEnd } from '@angular/router';
import { SpinnerLoadingService } from '../../common/services/spinner-loading-service';


@Component({
  selector: 'app-spinner-loading-indicator',
  standalone: true,
  imports: [
    CommonModule,
    // MatProgressSpinnerModule  // Uncomment if using Material
  ],
  templateUrl: './spinner-loading-indicator-component.component.html',
  styleUrls: ['./spinner-loading-indicator-component.component.css']
})
export class SpinnerLoadingIndicatorComponent implements OnInit {
  loading$: Observable<boolean>;

  @Input()
  detectRouteTransitions = false;

  @ContentChild('loading')
  customLoadingIndicator: TemplateRef<any> | null = null;

  constructor(    
    private router: Router,
    private loadingService: SpinnerLoadingService
  ) 
  {
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit() {
    if (this.detectRouteTransitions) {
      this.router.events.subscribe((event: any) => {
        if (event instanceof RouteConfigLoadStart) {
          this.loadingService.loadingOn();
        } else if (event instanceof RouteConfigLoadEnd) {
          this.loadingService.loadingOff();
        }
      });
    }
  }
}