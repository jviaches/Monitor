import { Injectable } from '@angular/core';
import { GeneralService } from './general.service';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserModel } from '../models/user.model';

// source: https://aws-amplify.github.io/docs/js/authentication

@Injectable()
export class AuthenticationService {

  private currentUserSubject: BehaviorSubject<UserModel>;
  public currentUser: Observable<UserModel>;

  // public allModules: ModuleModel[];

  constructor(private http: HttpClient, private generalService: GeneralService, private router: Router) {
    this.currentUserSubject = new BehaviorSubject<UserModel>(JSON.parse(localStorage.getItem('user')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  // public getAllModules(): ModuleModel[] {
  //   const mods: string = localStorage.getItem('allModules');

  //   if (mods && mods !== 'undefined') {
  //     return JSON.parse(mods);
  //   } else {
  //     localStorage.setItem('allModules', JSON.stringify(this.allModules));
  //     return this.allModules;
  //   }
  // }

  // public getModulesList() {
  //   this.http.get(this.generalService.URL + `/modules/get-all`)
  //     .subscribe(data => {
  //       const serverReply: ServerReply = data as ServerReply;

  //       this.allModules = JSON.parse(serverReply.payload);
  //       localStorage.setItem('allModules', JSON.stringify(this.allModules));
  //     },
  //       error => {
  //         this.generalService.showModalMessage('Login details are invalid');
  //       });
  // }

  public get currentUserValue(): UserModel {
    return this.currentUserSubject.value;
  }

  signUp(email: string, password: string) {
    const request = {
      Email: email,
      Password: password
    };

    const header = new HttpHeaders().set('Content-type', 'application/json');

    this.http.post<any>(this.generalService.URL + 'users/signup', request, { headers: header})
      .subscribe(data => {
        this.router.navigate(['/user-confirmation/1']);
      });
  }

  login(email: string, password: string) {
    // password = sha512_256(password);
    const request = {
      Email: email,
      Password: password
    };

    const header = new HttpHeaders().set('Content-type', 'application/json');

    this.http.post<any>(this.generalService.URL + 'users/signin', request, { headers: header})
      .subscribe(data => {

        // if (serverReply.payload === 'REQUIRE_ACTIVATION') {
        //   this.generalService.showModalMessage('Account require activation!').subscribe(() => {
        //     this.router.navigate(['/account-confirm']);
        //   });
        // }

        // if (serverReply.payload === 'ERROR') {
        //   this.generalService.showModalMessage('Email - password combination is not valid');
        //   return;
        // }

        const user: UserModel = data as UserModel;

        if (user && user.token) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSubject.next(user);
          // this.getModulesList();
        }
        this.router.navigate(['/dashboard']);
      },
        error => () => { this.generalService.showModalMessage('Error', 'Email - password combination is not valid'); });
  }

  updateUser(newUser: UserModel) {
    localStorage.setItem('user', JSON.stringify(newUser));
    this.currentUserSubject.next(newUser);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  isUserLoggedIn(): boolean {
    const currentUser = this.currentUserValue;
    if (currentUser && currentUser.token) {
      // logged in so return true
      return true;
    }

    return false;
  }
}
