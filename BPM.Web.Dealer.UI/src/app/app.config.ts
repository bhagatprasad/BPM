import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app.routes';
import { tokenInterceptor } from './interceptor/token-interceptor';
import { provideToastr, ToastrService } from '@iqx-limited/ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideAnimations(), // ✅ Add this - required for toastr animations
    provideHttpClient(withInterceptors([tokenInterceptor])),
    provideToastr({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
      closeButton: true,
      progressBar: true,
      newestOnTop: true,
      tapToDismiss: true,
    }),
    ToastrService // ✅ Add this if needed
  ]
};