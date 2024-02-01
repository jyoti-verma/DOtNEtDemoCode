import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSortModule } from '@angular/material/sort';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatListModule } from '@angular/material/list';
import { TableComponent } from './Component/table/table.component';
import { HttpClientModule } from '@angular/common/http';
import { MatDialogModule } from '@angular/material/dialog';
import { OpenDialogComponent } from './Component/open-dialog/open-dialog.component';
import { CreateLetterComponent } from './Component/create-letter/create-letter.component';
import { LetterComponent } from './Pages/letter/letter.component';
import { ToolbarComponent } from './Component/toolbar/toolbar.component';




@NgModule({
  declarations: [
    AppComponent,
    TableComponent,
    OpenDialogComponent,
    CreateLetterComponent,
    LetterComponent,
    ToolbarComponent
  ],
  imports: [
    BrowserModule, ReactiveFormsModule, FormsModule,
    MatTableModule, MatGridListModule, MatListModule, MatSortModule, MatDialogModule,
    MatInputModule, MatProgressSpinnerModule,
    MatSelectModule, MatToolbarModule,
    MatCardModule, MatPaginatorModule,
    MatButtonModule, MatMenuModule, MatIconModule,
    BrowserAnimationsModule, HttpClientModule
  ],

  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
