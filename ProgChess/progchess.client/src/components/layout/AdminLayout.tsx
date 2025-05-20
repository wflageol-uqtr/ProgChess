import AppSidebar from "../AppSidebar";
import { SidebarProvider, SidebarTrigger } from "../ui/sidebar";

export default function AdminLayout({ children }: any) {
  return (
    <div className="flex bg-zinc-900 text-white">
      <SidebarProvider>
        <AppSidebar />
        <main className="w-full">
          <SidebarTrigger />
          {children}
        </main>
      </SidebarProvider>
    </div>
  );
}
