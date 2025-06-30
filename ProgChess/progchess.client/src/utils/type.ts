
export type UnitTest = {
  id: number;
  code: string;
  isActive: boolean;
};

export type Exercise = {
  id: number;
  situation: string;
  baseCode: string;
  unitTests: UnitTest[];
  studentExercises: StudentExercice[]
};

export type TestResult = {
  $type: string,
  testName: string,
  success: boolean,
  expected?: string,
  actual?: string
}

export type Score = {
  id: number,
  permanentCode: string,
  exercise: Exercise,
  student: Student,
  scoreValue: number,
  answer: string
}

export type Student = {
  id: number,
  permanentCode: string,
  createdAt: string,
  updatedAt: string
}

export type StudentExercice = {
  exerciseId: number,
  exercise?: Exercise,
  studentId: 1,
  student: Student,
  isComplete: boolean,
}

export type Error = {
  status?: number,
  message?: string 
}

export type TabType = {
  id: string,
  name: string
  isActive: boolean,
  component: React.ReactElement
}