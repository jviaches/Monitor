// import 'hammerjs';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';
import { environment } from 'src/environments/environment';

// Amplify Configuration
import Auth from '@aws-amplify/auth';
import Storage from '@aws-amplify/storage';
import AWSConfig from 'src/aws-exports';

// Storage.configure(AWSConfig);
Auth.configure(AWSConfig);

// End Amplify Configuration
if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
