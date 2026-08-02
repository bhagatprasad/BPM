import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SpinnerLoadingService {
  private loadingSubject = new BehaviorSubject<boolean>(false);
  loading$ = this.loadingSubject.asObservable();
  
  // Counter to handle multiple concurrent requests
  private loadingCounter = 0;
  
  // Optional: Loading message
  private messageSubject = new BehaviorSubject<string>('');
  message$ = this.messageSubject.asObservable();

  constructor() {
    console.log('SpinnerLoadingService initialized');
  }

  /**
   * Show the spinner
   * @param message Optional message to display with spinner
   */
  show(message?: string): void {
    this.loadingCounter++;
    console.log(`🔵 Spinner show called. Counter: ${this.loadingCounter}`);
    
    if (message) {
      this.messageSubject.next(message);
    }
    
    this.loadingSubject.next(true);
  }

  /**
   * Hide the spinner
   * Uses counter to ensure spinner stays visible for all requests
   */
  hide(): void {
    if (this.loadingCounter > 0) {
      this.loadingCounter--;
      console.log(`🔵 Spinner hide called. Counter: ${this.loadingCounter}`);
    }
    
    if (this.loadingCounter === 0) {
      this.loadingSubject.next(false);
      this.messageSubject.next('');
    }
  }

  /**
   * Force hide the spinner (use cautiously)
   */
  forceHide(): void {
    this.loadingCounter = 0;
    this.loadingSubject.next(false);
    this.messageSubject.next('');
    console.log('⚠️ Spinner force hidden');
  }

  /**
   * Get current loading state
   */
  get isLoading(): boolean {
    return this.loadingSubject.value;
  }

  /**
   * Reset the spinner state (use for error recovery)
   */
  reset(): void {
    this.loadingCounter = 0;
    this.loadingSubject.next(false);
    this.messageSubject.next('');
    console.log('🔄 Spinner reset');
  }
}