
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
  studentExercise: StudentExercice,
  scoreValue: number,
  answer: string
}

export type StudentExercice = {
  id: number,
  exerciseId: number,
  exercise?: Exercise,
  studentPermanentCode: string,
  isComplete: boolean,
  createdAt: string,
  updatedAt: string
}

export type StudentExerciseGroup = {
  id: number,
  exercises: Exercise[],
  studentPermanentCode: string,
  createdAt: string,
  updatedAt: string
}

export type Error = {
  status?: number,
  message?: string 
}

export type TabType = {
  id: number,
  name: string
  isActive: boolean,
  component: React.ReactElement
}

export type Image = {
  id: number,
  userId: number,
  name: string,
  path: string
}