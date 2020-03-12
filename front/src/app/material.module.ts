import { NgModule } from '@angular/core';
import {MatCardModule} from '@angular/material/card';
import {MatTabsModule} from '@angular/material/tabs';
import {MatMenuModule} from '@angular/material/menu';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatSliderModule} from '@angular/material/slider';
import {MatIconModule} from '@angular/material/icon';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatTableModule} from '@angular/material/table';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatSortModule} from '@angular/material/sort';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatDialogModule} from '@angular/material/dialog';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatRadioModule} from '@angular/material/radio';

@NgModule({
  imports: [
    MatCardModule,
    MatTabsModule,
    MatMenuModule,
    MatExpansionModule,
    MatSliderModule,
    MatIconModule,
    MatPaginatorModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatSortModule,
    MatTooltipModule,
    MatDialogModule,
    MatDatepickerModule,
    // MatNativeDateModule,
    MatSlideToggleModule,
    MatGridListModule,
    MatRadioModule,
  ],
  exports: [
    MatCardModule,
    MatTabsModule,
    MatMenuModule,
    MatExpansionModule,
    MatSliderModule,
    MatIconModule,
    MatPaginatorModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatSortModule,
    MatTooltipModule,
    MatDialogModule,
    MatDatepickerModule,
    // MatNativeDateModule,
    MatSlideToggleModule,
    MatGridListModule,
    MatRadioModule,
  ],
  providers: [
    MatDatepickerModule,
  ],
})
export class MaterialModule { }

