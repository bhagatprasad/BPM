import { Component, ContentChild, Input, TemplateRef, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, Subscription } from 'rxjs';
import { Router, RouteConfigLoadStart, RouteConfigLoadEnd } from '@angular/router';
import { SpinnerLoadingService } from '../../common/services/spinner-loading-service';

@Component({
  selector: 'app-spinner-loading-indicator',
  standalone: true,
  imports: [
    CommonModule,
  ],
  templateUrl: './spinner-loading-indicator-component.component.html',
  styleUrls: ['./spinner-loading-indicator-component.component.css']
})
export class SpinnerLoadingIndicatorComponent implements OnInit, OnDestroy {
  hide() {
    throw new Error('Method not implemented.');
  }
  show(arg0: string) {
    throw new Error('Method not implemented.');
  }
  loading$: Observable<boolean>;
  loadingMessage: string = 'Loading...';
  private messageSubscription?: Subscription;

  @Input()
  detectRouteTransitions = false;

  @Input()
  defaultMessage: string = 'Loading...';

  @ContentChild('loading')
  customLoadingIndicator: TemplateRef<any> | null = null;

  constructor(    
    private router: Router,
    private loadingService: SpinnerLoadingService
  ) {
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit() {
    // Subscribe to message changes
    this.messageSubscription = this.loadingService.message$.subscribe(message => {
      this.loadingMessage = message || this.defaultMessage;
    });

    if (this.detectRouteTransitions) {
      this.router.events.subscribe((event: any) => {
        if (event instanceof RouteConfigLoadStart) {
          this.loadingService.show('Loading page...');
        } else if (event instanceof RouteConfigLoadEnd) {
          this.loadingService.hide();
        }
      });
    }
  }

  ngOnDestroy() {
    if (this.messageSubscription) {
      this.messageSubscription.unsubscribe();
    }
  }
}