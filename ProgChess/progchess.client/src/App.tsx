import AuthProvider from "./providers/AuthProvider";
import Routes from "./routes";

export default function App() {
  return (
    <AuthProvider>
      <Routes />
    </AuthProvider>
  );
}
